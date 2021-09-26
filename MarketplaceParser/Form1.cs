using OfficeOpenXml;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Keys = OpenQA.Selenium.Keys;

namespace WildberriesParser
{
    public partial class Form1 : Form
    {
        private Dictionary<string, FieldInfo> WBFields = new Dictionary<string, FieldInfo>();

        private Dictionary<string, string> WBSort = new Dictionary<string, string>();

        private Dictionary<string, FieldInfo> ozonFields = new Dictionary<string, FieldInfo>();

        private Dictionary<string, string> ozonSort = new Dictionary<string, string>();

        private Dictionary<string, FieldInfo> beruFields = new Dictionary<string, FieldInfo>();

        private Dictionary<string, string> beruSort = new Dictionary<string, string>();

        private List<Thread> parsers = new List<Thread>();

        private int maxCount = 0;

        private int count = 0;

        private object lockObj = new object();

        private ExcelPackage package;

        private bool commonStop = false;

        private bool stop = false;

        private bool saved = false;

        private List<int> specialSymbols = new List<int>() { 36, 38, 43, 44, 47, 58, 59, 61, 63, 64, 32, 34, 60, 62, 35, 37, 123, 125, 124, 92, 94, 126, 91, 93, 96 };

        public Form1()
        {
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.ProcessName == "chromedriver" || process.ProcessName == "chrome")
                    {
                        process.Kill();
                    }
                }
                catch { }
            }

            HttpClient client = new HttpClient(new HttpClientHandler() { AllowAutoRedirect = true });

            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            InitializeComponent();

            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create);

            toolTip1.SetToolTip(WBPictureBox, "https://wildberries.ru");
            toolTip1.SetToolTip(ozonPictureBox, "https://ozon.ru");
            toolTip1.SetToolTip(beruPictureBox, "https://pokupki.market.yandex.ru");

            int delta = searchButton.Left - queryTextBox.Right;
            searchButton.Left = this.ClientSize.Width - searchButton.Width - queryTextBox.Left + 2;
            queryTextBox.Width = searchButton.Left - queryTextBox.Left - delta + 2;
            searchButton.Height = queryTextBox.Height + 2;
            searchButton.Top = queryTextBox.Top - 1;
            tableLayoutPanel1.Left = 12;
            tableLayoutPanel1.Top = queryTextBox.Bottom + 12;
            tableLayoutPanel1.Width = this.ClientSize.Width - 24;
            tableLayoutPanel1.Height = this.ClientSize.Height - tableLayoutPanel1.Top - 12;

            queryTextBox.GotFocus += TextBox1_GotFocus;
            queryTextBox.LostFocus += TextBox1_LostFocus;
            WBCountTextBox.GotFocus += WBCountTextBox_GotFocus;
            WBCountTextBox.LostFocus += WBCountTextBox_LostFocus;
            ozonCountTextBox.GotFocus += OzonCountTextBox_GotFocus;
            ozonCountTextBox.LostFocus += OzonCountTextBox_LostFocus;
            beruCountTextBox.GotFocus += BeruCountTextBox_GotFocus;
            beruCountTextBox.LostFocus += BeruCountTextBox_LostFocus;

            SetWBDictonaries();
            SetOzonDictonaries();
            SetBeruDictonaries();

            queryTextBox.Tag = false;
            WBCountTextBox.Tag = false;
            ozonCountTextBox.Tag = false;
            beruCountTextBox.Tag = false;

            WBComboBox.SelectedIndex = 0;
            ozonComboBox.SelectedIndex = 0;
            beruComboBox.SelectedIndex = 0;
        }

        private ChromeDriver CreateDriver()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-position=-20000,-20000");
            options.AddArgument("--no-sandbox");
            options.AddArgument("no-sandbox");

            while (true)
            {
                try
                {
                    return new ChromeDriver(service, options);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("session not created"))
                    {
                        this.Invoke(new Action(() =>
                        {
                            this.WindowState = FormWindowState.Normal;
                            this.TopMost = true;
                            this.TopMost = false;
                            MessageBox.Show("Версия хрома и драйвера различаются", "Проблема", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                }
            }
        }

        private void SetWBDictonaries()
        {
            string[] texts = new[] { "Наименование запроса", "Товар", "Бренд", "Цена до скидки", "Цена после скидок", "Вес без упаковки, гр.", "Цена 100гр.", "Продажи", "Отзывы", "Артикул" };
            string[] names = new[] { "query", "name", "brand", "pricebefore", "priceafter", "weight", "price", "sales", "comments", "article" };
            for (int i = 0; i < texts.Length; i++)
            {
                WBFields.Add(names[i], new FieldInfo(i + 1, texts[i]));
            }
            string[] sortRus = new[] { "Популярность", "Рейтинг", "Цена 🠕", "Цена 🠗", "Скидка", "Обновление" };
            string[] sort = new[] { "popular", "rate", "priceup", "pricedown", "sale", "newly" };
            for (int i = 0; i < sort.Length; i++)
            {
                WBSort.Add(sortRus[i], sort[i]);
            }
        }

        private void SetOzonDictonaries()
        {
            string[] texts = new[] { "Наименование запроса", "Товар", "Бренд", "Цена до скидки", "Цена после скидок", "Вес без упаковки, гр.", "Цена 100гр.", "Отзывы", "Артикул" };
            string[] names = new[] { "query", "name", "brand", "pricebefore", "priceafter", "weight", "price", "comments", "article" };
            for (int i = 0; i < texts.Length; i++)
            {
                ozonFields.Add(names[i], new FieldInfo(i + 1, texts[i]));
            }
            string[] sortRus = new[] { "Популярность", "Новинки", "Цена 🠕", "Цена 🠗", "Скидка", "Рейтинг" };
            string[] sort = new[] { "popular", "new", "price", "price_desc", "discount", "rating" };
            for (int i = 0; i < sort.Length; i++)
            {
                ozonSort.Add(sortRus[i], sort[i]);
            }
        }

        private void SetBeruDictonaries()
        {
            string[] texts = new[] { "Наименование запроса", "Товар", "Бренд", "Цена до скидки", "Цена после скидок", "Вес без упаковки, гр.", "Цена 100гр.", "Отзывы", "Артикул", "Продажи", "Интерес" };
            string[] names = new[] { "query", "name", "brand", "pricebefore", "priceafter", "weight", "price", "comments", "article", "sales", "interest" };
            for (int i = 0; i < texts.Length; i++)
            {
                beruFields.Add(names[i], new FieldInfo(i + 1, texts[i]));
            }
            string[] sortRus = new[] { "Популярность", "Цена 🠕", "Цена 🠗", "Оценка", "Отзывы", "Скидка" };
            string[] sort = new[] { "popular", "aprice", "dprice", "quality", "opinions", "discount_p" };
            for (int i = 0; i < sort.Length; i++)
            {
                beruSort.Add(sortRus[i], sort[i]);
            }
        }

        private void BeruCountTextBox_LostFocus(object sender, EventArgs e)
        {
            beruCountTextBox.Tag = !string.IsNullOrEmpty(beruCountTextBox.Text);
            if (!(bool)beruCountTextBox.Tag)
            {
                beruCountTextBox.Text = "Все";
            }
        }

        private void BeruCountTextBox_GotFocus(object sender, EventArgs e)
        {
            if (!(bool)beruCountTextBox.Tag)
            {
                beruCountTextBox.Tag = true;
                beruCountTextBox.Text = "";
            }
        }

        private void OzonCountTextBox_LostFocus(object sender, EventArgs e)
        {
            ozonCountTextBox.Tag = !string.IsNullOrEmpty(ozonCountTextBox.Text);
            if (!(bool)ozonCountTextBox.Tag)
            {
                ozonCountTextBox.Text = "Все";
            }
        }

        private void OzonCountTextBox_GotFocus(object sender, EventArgs e)
        {
            if (!(bool)ozonCountTextBox.Tag)
            {
                ozonCountTextBox.Tag = true;
                ozonCountTextBox.Text = "";
            }
        }

        private void WBCountTextBox_LostFocus(object sender, EventArgs e)
        {
            WBCountTextBox.Tag = !string.IsNullOrEmpty(WBCountTextBox.Text);
            if (!(bool)WBCountTextBox.Tag)
            {
                WBCountTextBox.Text = "Все";
            }
        }

        private void WBCountTextBox_GotFocus(object sender, EventArgs e)
        {
            if (!(bool)WBCountTextBox.Tag)
            {
                WBCountTextBox.Tag = true;
                WBCountTextBox.Text = "";
            }
        }

        private void TextBox1_LostFocus(object sender, EventArgs e)
        {
            queryTextBox.Tag = !string.IsNullOrEmpty(queryTextBox.Text);
            if (!(bool)queryTextBox.Tag)
            {
                queryTextBox.Text = "Введите запрос...";
            }
        }

        private void TextBox1_GotFocus(object sender, EventArgs e)
        {
            if (!(bool)queryTextBox.Tag)
            {
                queryTextBox.Tag = true;
                queryTextBox.Text = "";
            }
        }

        private void StartCommonProgress()
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    string d = "";
                    string prev = "";
                    while (!commonStop)
                    {
                        if (maxCount == 0)
                        {
                            if (prev != "0")
                            {
                                commonProgressBar.Invoke(new Action(() =>
                                {
                                    commonProgressBar.Value = 0;
                                    label10.Text = "0%";
                                }));

                                prev = "0";
                            }
                        }
                        else
                        {
                            d = Math.Round(100.0 * count / maxCount, 1).ToString();
                            if (prev != d)
                            {
                                commonProgressBar.Invoke(new Action(() =>
                                {
                                    commonProgressBar.Value = count * 100 / maxCount;
                                    label10.Text = d.Replace(",", ".") + "%";
                                }));

                                prev = d;
                            }
                        }
                        if (parsers.Count == 0)
                        {
                            break;
                        }
                    }
                    if (maxCount == 0)
                    {
                        if (prev != "0")
                        {
                            commonProgressBar.Invoke(new Action(() =>
                            {
                                commonProgressBar.Value = 0;
                                label10.Text = "0%";
                            }));

                            prev = "0";
                        }
                    }
                    else
                    {
                        d = Math.Round(100.0 * count / maxCount, 1).ToString();
                        if (prev != d)
                        {
                            commonProgressBar.Invoke(new Action(() =>
                            {
                                commonProgressBar.Value = count * 100 / maxCount;
                                label10.Text = d.Replace(",", ".") + "%";
                            }));

                            prev = d;
                        }
                    }
                    if (!commonStop)
                    {
                        this.Invoke(new Action(() =>
                        {

                            if (package.Workbook.Worksheets.Count > 0)
                            {
                                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                                {
                                    if (!File.Exists(saveFileDialog1.FileName))
                                    {
                                        using (File.Create(saveFileDialog1.FileName))
                                        { }
                                    }
                                    package.SaveAs(new FileInfo(saveFileDialog1.FileName));
                                    saved = true;
                                    try
                                    {
                                        Process.Start(saveFileDialog1.FileName);
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            Process.Start("explorer.exe", new FileInfo(saveFileDialog1.FileName).DirectoryName);
                                        }
                                        catch
                                        { }
                                    }
                                    MessageBox.Show("Данные успешно сохранены", "Парсинг окончен", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    if (!Directory.Exists("outFiles"))
                                        Directory.CreateDirectory("outFiles");
                                    string path = Path.Combine("outFiles", "Marketplaces " + DateTime.Now.ToString("MM_dd_yyyy HH-mm-ss") + ".xlsx");
                                    using (File.Create(path))
                                    { }
                                    package.SaveAs(new FileInfo(path));
                                    saved = true;
                                }
                            }
                        }));
                        this.Invoke(new Action(() =>
                        {
                            commonProgressBar.Value = 0;
                            beruProgressBar.Value = 0;
                            ozonProgressBar.Value = 0;
                            WBProgressBar.Value = 0;
                            commonProgressBar.Visible = true;
                            beruProgressBar.Visible = true;
                            ozonProgressBar.Visible = true;
                            WBProgressBar.Visible = true;
                            label7.Text = "0%";
                            label8.Text = "0%";
                            label9.Text = "0%";
                            label10.Text = "0%";
                            label7.Visible = true;
                            label8.Visible = true;
                            label9.Visible = true;
                            label10.Visible = true;
                            label10.Visible = true;
                        }));
                    }
                }
                catch
                {
                    try
                    {
                        if (package.Workbook.Worksheets.Count > 0)
                        {
                            if (!Directory.Exists("outFiles"))
                                Directory.CreateDirectory("outFiles");
                            string path = Path.Combine("outFiles", "Marketplaces " + DateTime.Now.ToString("MM_dd_yyyy HH-mm-ss") + ".xlsx");
                            using (File.Create(path))
                            { }
                            package.SaveAs(new FileInfo(path));
                            saved = true;
                        }
                    }
                    catch { }
                }
                try
                {
                    count = 0;
                    maxCount = 0;
                    searchButton.Invoke(new Action(() =>
                    {
                        searchButton.Text = "Поиск";
                        searchButton.BackColor = Color.FromArgb(0, 192, 0);
                    }));
                }
                catch { }
            });
            thread.Start();
        }

        private string GetFormattedString(string query)
        {
            string ans = "";
            foreach (var q in query)
            {
                if (specialSymbols.Contains((int)q))
                {
                    ans += "%" + Convert.ToString((int)q, 16);
                }
                else
                {
                    ans += q;
                }
            }
            return ans;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (searchButton.Text == "Поиск")
            {
                string query = queryTextBox.Text;
                if (parsers.Count != 0)
                {
                    MessageBox.Show("Ещё не окончен прошлый парсинг", "Подождите", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!WBActiveCheckBox.Checked && !ozonActiveCheckBox.Checked && !beruActiveCheckBox.Checked)
                {
                    MessageBox.Show("Не выбраны площадки для парсинга", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!(bool)queryTextBox.Tag || string.IsNullOrEmpty(queryTextBox.Text))
                {
                    MessageBox.Show("Вы ввели пустой запрос\nВведите запрос", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int maxCount1 = 0;
                if (WBActiveCheckBox.Checked)
                {
                    if ((bool)WBCountTextBox.Tag)
                    {
                        if (int.TryParse(WBCountTextBox.Text, out int res))
                        {
                            if (res >= 1)
                            {
                                maxCount1 = res;
                            }
                        }
                    }
                    else
                    {
                        maxCount1 = -1;
                    }
                }
                int maxCount2 = 0;
                if (ozonActiveCheckBox.Checked)
                {
                    if ((bool)ozonCountTextBox.Tag)
                    {
                        if (int.TryParse(ozonCountTextBox.Text, out int res))
                        {
                            if (res >= 1)
                            {
                                maxCount2 = res;
                            }
                        }
                    }
                    else
                    {
                        maxCount2 = -1;
                    }
                }
                int maxCount3 = 0;
                if (beruActiveCheckBox.Checked)
                {
                    if ((bool)beruCountTextBox.Tag)
                    {
                        if (int.TryParse(beruCountTextBox.Text, out int res))
                        {
                            if (res >= 1)
                            {
                                maxCount3 = res;
                            }
                        }
                    }
                    else
                    {
                        maxCount3 = -1;
                    }
                }
                if (maxCount1 == 0 && maxCount2 == 0 && maxCount3 == 0)
                {
                    MessageBox.Show("Введено некорректное количество товаров\nВведите натуральное число", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (maxCount1 * maxCount2 * maxCount3 == 0)
                {
                    string s = "";
                    if (maxCount1 == 0 && WBActiveCheckBox.Checked)
                    {
                        if (s == "")
                        {
                            s += "Wildberries";
                        }
                    }
                    if (maxCount2 == 0 && ozonActiveCheckBox.Checked)
                    {
                        if (s != "")
                            s += ", ";
                        s += "ОЗОН";
                    }
                    if (maxCount3 == 0 && beruActiveCheckBox.Checked)
                    {
                        if (s != "")
                            s += ", ";
                        s += "Яндекс.Маркет";
                    }
                    if (s != "")
                    {
                        var result = MessageBox.Show($"Введено некорректное количество товаров({s})\nПродолжить?", "Предупреждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if (result != DialogResult.Yes)
                            return;
                    }
                }
                if (parsers.Count != 0)
                {
                    MessageBox.Show("Ещё не окончен прошлый парсинг", "Подождите", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (WBRemainsCheckBox.Checked)
                {
                    if (!WBFields.ContainsKey("available"))
                    {
                        WBFields.Add("available", new FieldInfo(WBFields.Count + 1, "Осталось"));
                    }
                }
                else
                {
                    WBFields.Remove("available");
                }
                package = new ExcelPackage();
                saved = false;
                label5.Visible = true;
                label6.Visible = true;
                commonProgressBar.Value = 0;
                parsers.Clear();
                if (maxCount1 != 0)
                {
                    WBProgressBar.Value = 0;
                    WBProgressBar.Visible = true;
                    label7.Visible = true;
                    parsers.Add(GetWBParser(query, maxCount1, WBSort[WBComboBox.Text], package));
                }
                else
                {
                    WBProgressBar.Visible = false;
                    label7.Visible = false;
                }
                if (maxCount2 != 0)
                {
                    ozonProgressBar.Value = 0;
                    ozonProgressBar.Visible = true;
                    label8.Visible = true;
                    parsers.Add(GetOzonParser(query, maxCount2, ozonSort[ozonComboBox.Text], package));
                }
                else
                {
                    ozonProgressBar.Visible = false;
                    label8.Visible = false;
                }
                if (maxCount3 != 0)
                {
                    beruProgressBar.Value = 0;
                    beruProgressBar.Visible = true;
                    label9.Visible = true;
                    parsers.Add(GetBeruParser(query, maxCount3, beruSort[beruComboBox.Text], package));
                }
                else
                {
                    beruProgressBar.Visible = false;
                    label9.Visible = false;
                }
                searchButton.Text = "Стоп";
                searchButton.BackColor = Color.FromArgb(246, 26, 26);
                stop = false;
                foreach (var thread in parsers)
                {
                    thread.Start();
                }
                commonStop = false;
                StartCommonProgress();
            }
            else
            {
                if (searchButton.Text == "Стоп")
                {
                    if (stop)
                    {
                        MessageBox.Show("Парсинг уже останаваливается", "Подождите", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (MessageBox.Show("Вы точно хотите остановить парсинг", "Опасность", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        stop = true;
                    }
                }
            }
        }

        private string GetCount(string url)
        {
            var driver = CreateDriver();
            driver.Manage().Window.Minimize();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            try
            {
                int n = 0;
                while (n < 3)
                {
                    n++;
                    while (true)
                    {
                        try
                        {
                            driver.Navigate().GoToUrl(url);
                            break;
                        }
                        catch
                        {
                            try
                            {
                                driver?.Close();
                                driver?.Quit();
                            }
                            catch { }
                            driver = CreateDriver();
                            driver.Manage().Window.Minimize();
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                        }
                    }
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    if (!driver.FindElementByTagName("html").GetAttribute("innerHTML").ToLower().Contains("добавить в корзину"))
                        return "0";
                    var elem = driver.FindElementByCssSelector("div[class='same-part-kt__order']").FindElement(By.CssSelector("button[class='btn-main']"));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", elem);
                    Thread.Sleep(200);
                    elem.Click();
                    Thread.Sleep(500);
                    if (!driver.PageSource.Contains("class=\"btn-base j-go-to-basket\""))
                    {
                        try
                        {
                            elem = driver.FindElementByCssSelector("button[class='c-btn-main']");
                            if (elem.Displayed)
                            {
                                elem.Click();
                                Thread.Sleep(500);
                                driver.FindElementByCssSelector("div[class='cart-btn-wrap']").Click();
                                Thread.Sleep(200);
                            }
                        }
                        catch { }
                    }
                    Thread.Sleep(300);
                    try
                    {
                        driver.Navigate().GoToUrl("https://lk.wildberries.ru/basket");
                        wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                        if (!driver.PageSource.Replace("'", "\"").Contains("class=\"i-empty-basket\""))
                            break;
                    }
                    catch
                    {
                    }
                }
                try
                {
                    var btnElement = driver.FindElementByCssSelector("button[class='count__plus plus']");
                    var brandElement = driver.FindElementByCssSelector("input[class='in_tb j-tb-qnt count__numeric ignore']");
                    while (true)
                    {
                        btnElement.Click();
                        if (btnElement.GetAttribute("class") != "count__plus plus")
                            break;
                        if (int.Parse(brandElement.GetProperty("value")) >= 255)
                            break;
                    }
                }
                catch
                { }
                try
                {
                    return driver.FindElementByCssSelector("input[class='in_tb j-tb-qnt count__numeric ignore']").GetProperty("value");
                }
                catch
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                string ozonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create), "OZON");
                if (!Directory.Exists(ozonPath))
                    Directory.CreateDirectory(ozonPath);
                using (var file = new StreamWriter(new FileStream(Path.Combine(ozonPath, "MarketPlaceParser_Log.txt"), FileMode.OpenOrCreate)))
                {
                    file.WriteLine(ex.Message + "\n" + driver.PageSource);
                }
            }
            finally
            {
                try
                {
                    driver?.Close();
                    driver?.Dispose();
                }
                catch { }
            }
            throw new Exception("Not found");
        }

        private Thread GetWBParser(string query, int maxCount, string sort, ExcelPackage package)
        {
            return new Thread(() =>
            {
                ChromeDriver driver = CreateDriver();
                driver.Manage().Window.Minimize();
                string prevCount = WBCountTextBox.Text;
                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    string request = "";
                    while (true)
                    {
                        try
                        {
                            driver.Navigate().GoToUrl($"https://www.wildberries.ru/catalog/0/search.aspx?search={GetFormattedString(query)}");
                            break;
                        }
                        catch
                        {
                            try
                            {
                                driver?.Close();
                                driver?.Quit();
                            }
                            catch { }
                            driver = CreateDriver();
                            driver.Manage().Window.Minimize();
                        }
                    }
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    request = driver.Url;
                    if (request.Contains("&sort="))
                    {
                        int n2 = request.IndexOf("&sort=");
                        string data2 = request.Substring(n2 + "&sort=".Length);
                        n2 = data2.IndexOf("&");
                        if (n2 > 0)
                        {
                            data2 = data2.Substring(0, n2);
                        }
                        request = request.Replace("&sort=" + data2, "");
                    }
                    if(driver.PageSource.Contains("Requests quota exceeded"))
                    {

                    }
                    if (request.Contains("search.aspx?"))
                    {
                        request += "&";
                    }
                    else
                    {
                        request += "?";
                    }
                    request += "sort=" + sort + "&xsearch=true";
                    while (true)
                    {
                        try
                        {
                            driver.Navigate().GoToUrl(request);
                            break;
                        }
                        catch
                        {
                            try
                            {
                                driver?.Close();
                                driver?.Quit();
                            }
                            catch { }
                            driver = CreateDriver();
                            driver.Manage().Window.Minimize();
                        }
                    }
                    if (driver.PageSource.Contains("Requests quota exceeded"))
                    {

                    }
                    int page = 1;
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    string html = driver.PageSource.Replace("&nbsp;", "").Replace("&thinsp;", "");
                    int n = html.IndexOf("<span class=\"goods-count\"");
                    string c = html.Substring(n + "<span class=\"goods-count\"".Length);
                    c = c.Substring(0, c.IndexOf("</span>"));
                    while ((n < 0 || int.Parse(c) <= 0) && stopwatch.Elapsed.TotalSeconds < 10)
                    {
                        html = driver.PageSource.Replace("&nbsp;", "").Replace("&thinsp;", "");
                        n = html.IndexOf("<span class=\"goods-count\"");
                        c = html.Substring(n + "<span class=\"goods-count\"".Length);
                        c = c.Substring(0, c.IndexOf("</span>"));
                        c = c.Substring(c.LastIndexOf(">") + 1);
                        for (int i = 0; i < c.Length; i++)
                        {
                            if (!char.IsDigit(c[i]))
                            {
                                c = c.Remove(i, 1);
                                i--;
                            }
                        }
                    }
                    if (n >= 0)
                    {
                        c = html.Substring(n + "<span class=\"goods-count\"".Length);
                        c = c.Substring(0, c.IndexOf("</span>"));
                        c = c.Substring(c.LastIndexOf(">") + 1);
                        for (int i = 0; i < c.Length; i++)
                        {
                            if (!char.IsDigit(c[i]))
                            {
                                c = c.Remove(i, 1);
                                i--;
                            }
                        }
                        n = int.Parse(c);
                        if (maxCount < 0 || n < maxCount)
                        {
                            maxCount = n;
                        }
                        if (maxCount > 5000)
                        {
                            maxCount = 5000;
                        }
                        this.Invoke(new Action(() =>
                        {
                            WBCountTextBox.Text = maxCount.ToString();
                            WBCountTextBox.Tag = true;
                        }));
                        lock (lockObj)
                        {
                            this.maxCount += maxCount;
                        }
                        int count = 0;
                        this.Invoke(new Action(() =>
                        {
                            if (maxCount == 0)
                            {
                                WBProgressBar.Value = 100;
                                label7.Text = "100%";
                            }
                            else
                            {
                                WBProgressBar.Value = 0;
                                label7.Text = "0%";
                            }
                        }));
                        ReadOnlyCollection<IWebElement> items;
                        List<string> urls = new List<string>();
                        string nextUrl;
                        char[] ar = new char[] { ' ', '\n', '\r' };
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("WB");
                        foreach (var item in WBFields.Values)
                        {
                            worksheet.SetValue(1, item.Column, item.Name);
                        }
                        string data;
                        ExcelRange cell;
                        IWebElement brandElement;
                        int price;
                        try
                        {
                            while (true)
                            {
                                if (stop)
                                    break;
                                try
                                {
                                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                                    Thread.Sleep(200);
                                    items = driver.FindElementsByCssSelector("a[class='product-card__main j-open-full-product-card']");
                                    urls.Clear();
                                    foreach (var item in items)
                                    {
                                        urls.Add(item.GetAttribute("href"));
                                    }
                                    items = driver.FindElementsByCssSelector("a[class='product-card__main j-open-full-product-card is-adult']");
                                    foreach (var item in items)
                                    {
                                        urls.Add(item.GetAttribute("href"));
                                    }
                                    if (driver.PageSource.Replace("'", "\"").Contains("<a class=\"pagination-next\""))
                                    {
                                        try
                                        {
                                            driver.FindElementByClassName("pagination-next").GetAttribute("href");
                                            page++;
                                            nextUrl = request + $"&page={page}";
                                        }
                                        catch
                                        {
                                            nextUrl = null;
                                        }
                                    }
                                    else
                                    {
                                        nextUrl = null;
                                    }
                                    foreach (var url in urls)
                                    {
                                        if (stop)
                                            break;
                                        try
                                        {
                                            if (url == null)
                                                continue;
                                            price = -1;
                                            while (true)
                                            {
                                                try
                                                {
                                                    driver.Navigate().GoToUrl(url);
                                                    break;
                                                }
                                                catch
                                                {
                                                    try
                                                    {
                                                        driver?.Close();
                                                        driver?.Quit();
                                                    }
                                                    catch { }
                                                    driver = CreateDriver();
                                                    driver.Manage().Window.Minimize();
                                                }
                                            }
                                            if (driver.PageSource.Contains("Requests quota exceeded"))
                                            {

                                            }
                                            html = "";
                                            while (!html.Contains("</div>"))
                                            {
                                                html = driver.PageSource.Replace("&nbsp;", " ").Replace("&thinsp;", " ");
                                            }
                                            worksheet.SetValue(count + 2, WBFields["query"].Column, query);
                                            if (html.Contains("<a id=\"brandBannerImgRef\""))
                                            {
                                                try
                                                {
                                                    brandElement = driver.FindElementById("brandBannerImgRef");
                                                    try
                                                    {
                                                        data = brandElement.GetAttribute("href");
                                                        cell = worksheet.Cells[count + 2, WBFields["brand"].Column];
                                                        cell.Hyperlink = new ExcelHyperLink(data, UriKind.Absolute) { Display = brandElement.GetAttribute("title") };
                                                    }
                                                    catch
                                                    { }
                                                }
                                                catch { }
                                            }
                                            n = html.IndexOf("class=\"same-part-kt__header\"");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    cell = worksheet.Cells[count + 2, WBFields["name"].Column];
                                                    var gt = driver.FindElementByCssSelector("h1[class='same-part-kt__header']").Text;
                                                    cell.Hyperlink = new ExcelHyperLink(url, UriKind.Absolute)
                                                    {
                                                        Display = driver.FindElementByCssSelector("h1[class='same-part-kt__header']").Text
                                                    };
                                                }
                                                catch { }
                                            }
                                            n = html.IndexOf("class=\"same-part-kt__article\"");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    html = html.Substring(n + "class=\"same-part-kt__article\"".Length);
                                                    data = html.Substring(0, html.IndexOf("</p>"));
                                                    data = data.Substring(0, data.LastIndexOf("</"));
                                                    data = data.Substring(data.LastIndexOf(">") + 1);
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (!char.IsDigit(data[i]))
                                                        {
                                                            data = data.Remove(i, 1);
                                                            i--;
                                                        }
                                                    }
                                                    try
                                                    {
                                                        worksheet.SetValue(count + 2, WBFields["article"].Column, int.Parse(data));
                                                    }
                                                    catch
                                                    {
                                                        worksheet.SetValue(count + 2, WBFields["article"].Column, data);
                                                    }
                                                }
                                                catch { }
                                            }
                                            n = html.IndexOf("<p class=\"same-part-kt__order-quantity j-orders-count-wrapper\"");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    html = html.Substring(n + "<p class=\"same-part-kt__order-quantity j-orders-count-wrapper\"".Length);
                                                    data = html.Substring(0, html.IndexOf("</span>")).Replace(" ", "");
                                                    while (true)
                                                    {
                                                        try
                                                        {
                                                            if (data.IndexOf("</script>") < 0)
                                                            {
                                                                break;
                                                            }
                                                            data = data.Substring(data.IndexOf("</script>") + "</script>".Length);
                                                            if (data.IndexOf("<script") != 0)
                                                            {
                                                                break;
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    try
                                                    {
                                                        data = data.Substring(0, data.IndexOf("<"));
                                                    }
                                                    catch { }
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (!char.IsDigit(data[i]))
                                                        {
                                                            data = data.Remove(i, 1);
                                                            i--;
                                                        }
                                                    }
                                                    worksheet.SetValue(count + 2, WBFields["sales"].Column, int.Parse(data));
                                                }
                                                catch
                                                {
                                                    worksheet.SetValue(count + 2, WBFields["sales"].Column, 0);
                                                }
                                            }
                                            else
                                            {
                                                worksheet.SetValue(count + 2, WBFields["sales"].Column, 0);
                                            }
                                            n = html.LastIndexOf("<span class=\"price-block__final-price\"");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    html = html.Substring(n + "<span class=\"price-block__final-price\"".Length);
                                                    data = html.Substring(0, html.IndexOf("</span>"));
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (!char.IsDigit(data[i]))
                                                        {
                                                            data = data.Remove(i, 1);
                                                            i--;
                                                        }
                                                    }
                                                    try
                                                    {
                                                        price = int.Parse(data);
                                                        worksheet.SetValue(count + 2, WBFields["priceafter"].Column, price);
                                                    }
                                                    catch { }
                                                }
                                                catch { }
                                            }
                                            n = html.LastIndexOf("class=\"price-block__old-price j-final-saving\"");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    html = html.Substring(n + "class=\"price-block__old-price j-final-saving\"".Length);
                                                    data = html.Substring(0, html.IndexOf("</del>"));
                                                    n = data.IndexOf(">");
                                                    if (n >= 0)
                                                    {
                                                        data = data.Substring(n + 1);
                                                        for (int i = 0; i < data.Length; i++)
                                                        {
                                                            if (!char.IsDigit(data[i]))
                                                            {
                                                                data = data.Remove(i, 1);
                                                                i--;
                                                            }
                                                        }
                                                        worksheet.SetValue(count + 2, WBFields["pricebefore"].Column, int.Parse(data));
                                                    }
                                                }
                                                catch { }
                                            }
                                            data = html;
                                            n = data.IndexOf("Вес товара без упаковки (г)");
                                            if (n < 0)
                                            {
                                                n = data.IndexOf("Вес товара с упаковкой (г)");
                                            }
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = data.Substring(n);
                                                    data = data.Substring(0, data.IndexOf("</tr>"));
                                                    n = data.IndexOf("<td");
                                                    if (n >= 0)
                                                    {
                                                        data = data.Substring(n + "<td".Length).Replace(" ", "");
                                                        while (true)
                                                        {
                                                            try
                                                            {
                                                                if (data.IndexOf("</script>") < 0)
                                                                {
                                                                    break;
                                                                }
                                                                data = data.Substring(data.IndexOf("</script>") + "</script>".Length);
                                                                if (data.IndexOf("<script") != 0)
                                                                {
                                                                    break;
                                                                }
                                                            }
                                                            catch
                                                            {
                                                                break;
                                                            }
                                                        }
                                                        try
                                                        {
                                                            data = data.Substring(0, data.IndexOf("<"));
                                                        }
                                                        catch { }
                                                        for (int i = 0; i < data.Length; i++)
                                                        {
                                                            if (!char.IsDigit(data[i]) && data[i] != '.')
                                                            {
                                                                data = data.Remove(i, 1);
                                                                i--;
                                                            }
                                                        }
                                                        worksheet.SetValue(count + 2, WBFields["weight"].Column, (int)double.Parse(data, CultureInfo.InvariantCulture));
                                                        if (price > 0)
                                                        {
                                                            try
                                                            {
                                                                worksheet.SetValue(count + 2, WBFields["price"].Column, price * 100 / (int)double.Parse(data, CultureInfo.InvariantCulture));
                                                            }
                                                            catch
                                                            { }
                                                        }
                                                    }
                                                }
                                                catch { }
                                            }
                                            n = html.IndexOf("<a id=\"a-Comments\"");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = driver.FindElementById("a-Comments").Text;
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (!char.IsDigit(data[i]))
                                                        {
                                                            data = data.Remove(i, 1);
                                                            i--;
                                                        }
                                                    }
                                                    worksheet.SetValue(count + 2, WBFields["comments"].Column, int.Parse(data));
                                                }
                                                catch
                                                {
                                                    worksheet.SetValue(count + 2, WBFields["comments"].Column, 0);
                                                }
                                            }
                                            else
                                            {
                                                worksheet.SetValue(count + 2, WBFields["comments"].Column, 0);
                                            }
                                        }
                                        catch
                                        { }
                                        try
                                        {
                                            if (WBFields.ContainsKey("available"))
                                            {
                                                data = GetCount(url);
                                                try
                                                {
                                                    worksheet.SetValue(count + 2, WBFields["available"].Column, int.Parse(data));
                                                }
                                                catch
                                                {
                                                    worksheet.SetValue(count + 2, WBFields["available"].Column, data);
                                                }
                                            }
                                        }
                                        catch { }
                                        count++;
                                        lock (lockObj)
                                        {
                                            this.count++;
                                        }
                                        this.Invoke(new Action(() =>
                                        {
                                            WBProgressBar.Value = 100 * count / maxCount;
                                            label7.Text = Math.Round(100.0 * count / maxCount, 1).ToString().Replace(",", ".") + "%";
                                        }));

                                        if (count == maxCount)
                                            break;
                                    }
                                    if (count == maxCount)
                                        break;
                                    if (nextUrl == null)
                                        break;
                                    while (true)
                                    {
                                        try
                                        {
                                            driver.Navigate().GoToUrl(nextUrl);
                                            break;
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                driver?.Close();
                                                driver?.Quit();
                                            }
                                            catch { }
                                            driver = CreateDriver();
                                            driver.Manage().Window.Minimize();
                                        }
                                    }

                                }
                                catch { }
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                this.Invoke(new Action(() =>
                                {
                                    MessageBox.Show(ex.Message, "Непредвиденная ошибка (Wildberries)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }));
                            }
                            catch { }
                        }
                        worksheet.Cells[1, 1, count + 1, WBFields.Count].AutoFitColumns();
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("По вашему запросу ничего не найдено (Wildberries)", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }));
                    }
                }
                catch { }
                try
                {
                    driver?.Close();
                    driver?.Quit();
                }
                catch { }
                this.Invoke(new Action(() =>
                {
                    WBCountTextBox.Text = int.TryParse(prevCount, out int a) ? prevCount : "Все";
                    WBCountTextBox.Tag = int.TryParse(prevCount, out a);
                }));
                parsers.Remove(Thread.CurrentThread);
            });
        }

        private Thread GetOzonParser(string query, int maxCount, string sort, ExcelPackage package)
        {
            return new Thread(() =>
            {
                ChromeDriver driver = CreateDriver();
                driver.Manage().Window.Minimize();
                string prevCount = ozonCountTextBox.Text;
                try
                {
                    string text = "";
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    string request = "";
                    while (true)
                    {
                        try
                        {
                            driver.Navigate().GoToUrl($"https://www.ozon.ru/search/?from_global=true&text={GetFormattedString(query)}");
                            break;
                        }
                        catch
                        {
                            try
                            {
                                driver?.Close();
                                driver?.Quit();
                            }
                            catch { }
                            driver = CreateDriver();
                            driver.Manage().Window.Minimize();
                        }
                    }
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    Thread.Sleep(1000);
                    while (driver.PageSource.Replace("'", "\"").Contains("<meta name=\"ROBOTS\""))
                    {
                        driver.Manage().Window.Position = new Point(0, 0);
                        driver.Manage().Window.Maximize();
                        MessageBox.Show("Подтвердите, что я не робот", "Помогите мне", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    driver.Manage().Window.Position = new Point(-20000, -20000);
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

                    request = driver.Url + $"&sorting={sort}";
                    while (true)
                    {
                        try
                        {
                            driver.Navigate().GoToUrl(request);
                            break;
                        }
                        catch
                        {
                            try
                            {
                                driver?.Close();
                                driver?.Quit();
                            }
                            catch { }
                            driver = CreateDriver();
                            driver.Manage().Window.Minimize();
                        }
                    }
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

                    Thread.Sleep(1000);
                    while (driver.PageSource.Replace("'", "\"").Contains("<meta name=\"ROBOTS\""))
                    {
                        driver.Manage().Window.Position = new Point(0, 0);
                        driver.Manage().Window.Maximize();
                        MessageBox.Show("Подтвердите, что я не робот", "Помогите мне", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    driver.Manage().Window.Position = new Point(-20000, -20000);
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    string html = driver.PageSource.Replace("&nbsp;", "").Replace("&thinsp;", "").Replace(" ", "");
                    int n = html.IndexOf("data-widget=\"fulltextResultsHeader\"");
                    bool flag = false, flag2 = false;
                    if (n >= 0)
                    {
                        string c = html.Substring(n + "data-widget=\"fulltextResultsHeader\"".Length);
                        c = new string(c.Substring(0, c.IndexOf("</div>")).Reverse().ToArray());
                        string c2 = "";
                        for (int i = 0; i < c.Length; i++)
                        {
                            if (char.IsDigit(c[i]))
                            {
                                c2 = c[i] + c2;
                            }
                            else
                            {
                                if (c2 != "")
                                    break;
                            }
                        }
                        n = int.Parse(c2);
                        if (maxCount < 0 || n < maxCount)
                        {
                            maxCount = n;
                        }
                        if (maxCount > 9972)
                        {
                            maxCount = 9972;
                        }
                        this.Invoke(new Action(() =>
                        {
                            ozonCountTextBox.Text = maxCount.ToString();
                            ozonCountTextBox.Tag = true;
                        }));
                        lock (lockObj)
                        {
                            this.maxCount += maxCount;
                        }
                        int count = 0;
                        this.Invoke(new Action(() =>
                        {
                            if (maxCount == 0)
                            {
                                ozonProgressBar.Value = 100;
                                label8.Text = "100%";
                            }
                            else
                            {
                                ozonProgressBar.Value = 0;
                                label8.Text = "0%";
                            }
                        }));

                        ReadOnlyCollection<IWebElement> items, items2;
                        List<string> urls = new List<string>();
                        string nextUrl;
                        int page = 1;
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Озон");
                        foreach (var item in ozonFields.Values)
                        {
                            worksheet.SetValue(1, item.Column, item.Name);
                        }
                        string data;
                        ExcelRange cell;
                        IWebElement brandElement;
                        int price;
                        HashSet<string> pages = new HashSet<string>();
                        Stopwatch stopwatch = new Stopwatch();
                        try
                        {
                            string data2;
                            while (true)
                            {
                                if (stop)
                                {
                                    break;
                                }
                                try
                                {
                                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

                                    while (driver.PageSource.Replace("'", "\"").Contains("<meta name=\"ROBOTS\""))
                                    {
                                        driver.Manage().Window.Position = new Point(0, 0);
                                        driver.Manage().Window.Maximize();
                                        MessageBox.Show("Подтвердите, что я не робот", "Помогите мне", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    driver.Manage().Window.Position = new Point(-20000, -20000);
                                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete")); 
                                    Thread.Sleep(1000);
                                    html = driver.PageSource;
                                    n = html.IndexOf("data-widget=\"searchResultsV2\"");
                                    data = "f";
                                    if (!pages.Add(driver.Url))
                                    {
                                        break;
                                    }
                                    if (n >= 0)
                                    {
                                        data = html.Substring(n + "data-widget=\"searchResultsV2\"".Length);
                                        data = data.Substring(data.IndexOf("class=\"") + "class=\"".Length);
                                        data = data.Substring(data.IndexOf("<div"));
                                        data = data.Substring(data.IndexOf("class=\"") + "class=\"".Length);
                                        data = data.Substring(data.IndexOf("<a") + 2);
                                        data = data.Substring(data.IndexOf("class=\"") + "class=\"".Length);
                                        data = data.Substring(0, data.IndexOf("\""));
                                    }
                                    stopwatch.Restart();
                                    do
                                    {
                                        items = driver.FindElementsByCssSelector($"a[class='{data}']");
                                    }
                                    while (items.Count < 36 && items.Count + count < maxCount);
                                    stopwatch.Stop();
                                    urls.Clear();
                                    foreach (var item in items)
                                    {
                                        try
                                        {
                                            data = item.GetAttribute("href");
                                            if (data != null)
                                            {
                                                urls.Add(data);
                                            }
                                            else
                                            {
                                                item.Click();
                                                while (true)
                                                {
                                                    html = driver.PageSource;
                                                    if (html.LastIndexOf("</script>") < html.LastIndexOf("<div"))
                                                        break;
                                                }
                                                html = html.Substring(html.LastIndexOf("<button"));
                                                html = html.Substring(html.IndexOf("<div"));
                                                html = html.Substring(html.IndexOf("class=\"") + "class=\"".Length);
                                                html = html.Substring(0, html.IndexOf("\""));
                                                price = -1;
                                                brandElement = null;
                                                foreach (var elem in driver.FindElementsByCssSelector("div[class='vue-portal-target']"))
                                                {
                                                    n = driver.PageSource.Replace("'", "\"").IndexOf(elem.GetAttribute("innerHTML").Replace("'", "\""));
                                                    if (n > price)
                                                    {
                                                        price = n;
                                                        brandElement = elem;
                                                    }
                                                }
                                                brandElement.FindElement(By.CssSelector($"div[class='{html}']")).Click();
                                                wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                                                html = driver.PageSource;
                                                n = html.IndexOf("data-widget=\"searchResultsV2\"");
                                                if (n >= 0)
                                                {
                                                    data = html.Substring(n + "data-widget=\"searchResultsV2\"".Length);
                                                    data = data.Substring(data.IndexOf("class=\"") + "class=\"".Length);
                                                    data = data.Substring(data.IndexOf("<div"));
                                                    data = data.Substring(data.IndexOf("class=\"") + "class=\"".Length);
                                                    data = data.Substring(data.IndexOf("<a") + 2);
                                                    data = data.Substring(data.IndexOf("class=\"") + "class=\"".Length);
                                                    data = data.Substring(0, data.IndexOf("\""));
                                                }
                                                urls.Clear();
                                                stopwatch.Restart();
                                                do
                                                {
                                                    items2 = driver.FindElementsByCssSelector($"a[class='{data}']");
                                                }
                                                while (items2.Count < 36 && items2.Count + count < maxCount);
                                                stopwatch.Stop();
                                                foreach (var item2 in items2)
                                                {
                                                    try
                                                    {
                                                        data = item2.GetAttribute("href");
                                                        if (data != null)
                                                        {
                                                            urls.Add(data);
                                                        }
                                                    }
                                                    catch { }
                                                }
                                                break;
                                            }
                                        }
                                        catch
                                        { }
                                    }
                                    if (urls.Count == 0)
                                    {
                                        break;
                                    }
                                    nextUrl = null;
                                    page++;
                                    nextUrl = request + $"&page={page}";
                                    foreach (var url in urls)
                                    {
                                        if (stop)
                                            break;
                                        try
                                        {
                                            if (url == null)
                                            {
                                                continue;
                                            }
                                            price = -1;
                                            while (true)
                                            {
                                                try
                                                {
                                                    driver.Navigate().GoToUrl(url);
                                                    break;
                                                }
                                                catch
                                                {
                                                    try
                                                    {
                                                        driver?.Close();
                                                        driver?.Quit();
                                                    }
                                                    catch { }
                                                    try
                                                    {
                                                        driver = CreateDriver();
                                                        driver.Manage().Window.Minimize();
                                                    }
                                                    catch { }
                                                }
                                            }
                                            wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                                            while (driver.PageSource.Replace("'", "\"").Contains("<meta name=\"ROBOTS\""))
                                            {
                                                driver.Manage().Window.Position = new Point(0, 0);
                                                driver.Manage().Window.Maximize();
                                                MessageBox.Show("Подтвердите, что я не робот", "Помогите мне", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            }
                                            try
                                            {
                                                driver.Manage().Window.Position = new Point(-20000, -20000);
                                            }
                                            catch { }
                                            wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                                            html = "";

                                            while (!html.Contains("</div>"))
                                            {
                                                html = driver.PageSource.Replace("&nbsp;", " ").Replace("&thinsp;", " ").Replace("&quot;", "\"");
                                            }
                                            text = "";
                                            var height = (long)driver.ExecuteScript("return document.body.scrollHeight");
                                            for (int i = 0; i < height; i += 150)
                                            {
                                                height = (long)driver.ExecuteScript("return document.body.scrollHeight");
                                                driver.ExecuteScript("window.scrollTo(0," + i.ToString() + ")");
                                            }
                                            stopwatch.Restart();
                                            while (!text.ToLower().Contains("характеристики") && stopwatch.Elapsed.TotalSeconds < 15)
                                            {
                                                text = driver.FindElementByTagName("html").Text;
                                            }
                                            Thread.Sleep(100);
                                            worksheet.SetValue(count + 2, ozonFields["query"].Column, query);
                                            n = html.IndexOf("data-widget=\"webBrand\"");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = html.Substring(n + "data-widget=\"webBrand\"".Length);
                                                    data = data.Substring(data.IndexOf("<a"));
                                                    data = data.Substring(data.IndexOf("class=\"") + "class=\"".Length);
                                                    data = data.Substring(0, data.IndexOf("\""));
                                                    brandElement = driver.FindElementByCssSelector($"a[class='{data}']");
                                                    try
                                                    {
                                                        data = brandElement.GetAttribute("href");
                                                        data2 = text;
                                                        try
                                                        {
                                                            n = data2.IndexOf("Все товары ");
                                                            if (n >= 0)
                                                            {
                                                                data2 = data2.Substring(n + "Все товары ".Length);
                                                                try
                                                                {
                                                                    data2 = data2.Substring(0, data2.IndexOf("\r\n"));
                                                                }
                                                                catch { }
                                                            }
                                                            else
                                                            {
                                                                n = data2.LastIndexOf("Бренд\r\n");
                                                                if (n >= 0)
                                                                {
                                                                    data2 = data2.Substring(n + "Бренд\r\n".Length);
                                                                    for (int i = 0; i < data2.Length; i++)
                                                                    {
                                                                        if ((data2[i] == '\r' || data2[i] == '\n') && i != 0)
                                                                        {
                                                                            if (i != 0)
                                                                            {
                                                                                data2 = data2.Substring(0, i);
                                                                                break;
                                                                            }
                                                                            else
                                                                            {
                                                                                data2 = data2.Remove(i, 1);
                                                                                i--;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    n = data2.LastIndexOf("Бренд:\r\n");
                                                                    if (n >= 0)
                                                                    {
                                                                        data2 = data2.Substring(n + "Бренд:\r\n".Length);
                                                                        for (int i = 0; i < data2.Length; i++)
                                                                        {
                                                                            if ((data2[i] == '\r' || data2[i] == '\n') && i != 0)
                                                                            {
                                                                                if (i != 0)
                                                                                {
                                                                                    data2 = data2.Substring(0, i);
                                                                                    break;
                                                                                }
                                                                                else
                                                                                {
                                                                                    data2 = data2.Remove(i, 1);
                                                                                    i--;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        data2 = "";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        catch { }
                                                        cell = worksheet.Cells[count + 2, ozonFields["brand"].Column];
                                                        if (data2 != "")
                                                        {
                                                            cell.Hyperlink = new ExcelHyperLink(data, UriKind.Absolute) { Display = data2 };
                                                        }
                                                        else
                                                        {
                                                            cell.Hyperlink = new ExcelHyperLink(data, UriKind.Absolute);
                                                        }

                                                    }
                                                    catch
                                                    { }
                                                }
                                                catch { }
                                            }
                                            data = html;
                                            n = data.IndexOf("data-widget=\"webProductHeading\"");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = data.Substring(n);
                                                    data = data.Substring(data.IndexOf("<h1"));
                                                    data = data.Substring(data.IndexOf(">") + 1);
                                                    data = data.Substring(0, data.IndexOf("</h1>"));
                                                    cell = worksheet.Cells[count + 2, ozonFields["name"].Column];
                                                    cell.Hyperlink = new ExcelHyperLink(url, UriKind.Absolute) { Display = data };
                                                }
                                                catch
                                                { }
                                            }
                                            data = text;
                                            n = data.IndexOf("Код товара:");
                                            if (n < 0)
                                            {
                                                n = data.IndexOf("Код:");
                                            }
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = data.Substring(n);
                                                    data = data.Substring(0, data.IndexOf("\r\n"));
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (!char.IsDigit(data[i]))
                                                        {
                                                            data = data.Remove(i, 1);
                                                            i--;
                                                        }
                                                    }
                                                    try
                                                    {
                                                        worksheet.SetValue(count + 2, ozonFields["article"].Column, int.Parse(data));
                                                    }
                                                    catch
                                                    {
                                                        worksheet.SetValue(count + 2, ozonFields["article"].Column, data);
                                                    }
                                                }
                                                catch { }
                                            }
                                            flag2 = false;
                                            do
                                            {
                                                data = html;
                                                n = data.IndexOf("data-widget=\"webPrice\"");
                                                if (n >= 0)
                                                {
                                                    try
                                                    {
                                                        data = data.Substring(n + "data-widget=\"webPrice\"".Length);
                                                        n = data.IndexOf("<span");
                                                        flag = flag2 || (data.Contains("Товар закончился") && data.IndexOf("Товар закончился") < n);
                                                        if (n >= 0)
                                                        {
                                                            if (!flag)
                                                            {
                                                                data = data.Substring(data.IndexOf("</button>"));
                                                                n = data.IndexOf("<span");
                                                            }
                                                            data = data.Substring(n + "<span".Length);
                                                            data = data.Substring(data.IndexOf("<span"));
                                                            data = data.Substring(data.IndexOf(">") + 1);
                                                            data2 = data.Substring(0, data.IndexOf("</span>"));
                                                            try
                                                            {
                                                                data2 = data2.Substring(data2.IndexOf(">"));
                                                            }
                                                            catch { }
                                                            for (int i = 0; i < data2.Length; i++)
                                                            {
                                                                if (!char.IsDigit(data2[i]))
                                                                {
                                                                    data2 = data2.Remove(i, 1);
                                                                    i--;
                                                                }
                                                            }
                                                            try
                                                            {
                                                                price = int.Parse(data2);
                                                                worksheet.SetValue(count + 2, ozonFields["priceafter"].Column, price);
                                                                flag2 = false;
                                                            }
                                                            catch
                                                            {
                                                                if (flag2)
                                                                {
                                                                    break;
                                                                }
                                                                flag2 = true;
                                                                continue;
                                                            }
                                                            data = data.Substring(0, data.IndexOf("</div>"));
                                                            n = data.IndexOf("<span");
                                                            if (n >= 0)
                                                            {
                                                                data = data.Substring(n + "<span".Length);
                                                                data = data.Substring(data.IndexOf(">") + 1);
                                                                data2 = data.Substring(0, data.IndexOf("</span>"));
                                                                try
                                                                {
                                                                    data2 = data2.Substring(data2.IndexOf(">"));
                                                                }
                                                                catch { }
                                                                try
                                                                {
                                                                    for (int i = 0; i < data2.Length; i++)
                                                                    {
                                                                        if (!char.IsDigit(data2[i]))
                                                                        {
                                                                            data2 = data2.Remove(i, 1);
                                                                            i--;
                                                                        }
                                                                    }
                                                                    worksheet.SetValue(count + 2, ozonFields["pricebefore"].Column, int.Parse(data2));
                                                                }
                                                                catch
                                                                { }
                                                            }
                                                        }
                                                    }
                                                    catch { }
                                                }
                                            }
                                            while (flag2);
                                            data = text;
                                            n = data.LastIndexOf("Вес товара, г");
                                            if (n < 0)
                                            {
                                                n = data.LastIndexOf("Вес ");
                                            }
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = data.Substring(n);
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (!char.IsDigit(data[i]))
                                                        {
                                                            if ((data[i] == '\r' || data[i] == '\n') && i != 0)
                                                            {
                                                                data = data.Substring(0, i);
                                                                break;
                                                            }
                                                            data = data.Remove(i, 1);
                                                            i--;
                                                        }
                                                    }
                                                    worksheet.SetValue(count + 2, ozonFields["weight"].Column, int.Parse(data));
                                                    if (price > 0)
                                                    {
                                                        try
                                                        {
                                                            worksheet.SetValue(count + 2, ozonFields["price"].Column, price * 100 / int.Parse(data));
                                                        }
                                                        catch
                                                        { }
                                                    }
                                                }
                                                catch { }
                                            }
                                            data = text;
                                            n = data.IndexOf("отзыв");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = data.Substring(0, n);
                                                    data = data.Substring(Math.Max(data.LastIndexOf("\n"), data.LastIndexOf("\r")) + 1);
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (!char.IsDigit(data[i]))
                                                        {
                                                            data = data.Remove(i, 1);
                                                            i--;
                                                        }
                                                    }
                                                    worksheet.SetValue(count + 2, ozonFields["comments"].Column, int.Parse(data));
                                                }
                                                catch
                                                {
                                                    worksheet.SetValue(count + 2, ozonFields["comments"].Column, 0);
                                                }
                                            }
                                            else
                                            {
                                                worksheet.SetValue(count + 2, ozonFields["comments"].Column, 0);
                                            }
                                        }
                                        catch
                                        { }
                                        count++;
                                        lock (lockObj)
                                        {
                                            this.count++;
                                        }
                                        this.Invoke(new Action(() =>
                                        {
                                            ozonProgressBar.Value = 100 * count / maxCount;
                                            label8.Text = Math.Round(100.0 * count / maxCount, 1).ToString().Replace(",", ".") + "%";
                                        }));

                                        if (count == maxCount)
                                            break;
                                    }
                                    if (count == maxCount)
                                    {
                                        break;
                                    }
                                    if (nextUrl == null)
                                    {
                                        break;
                                    }
                                    while (true)
                                    {
                                        try
                                        {
                                            driver.Navigate().GoToUrl(request);
                                            break;
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                driver?.Close();
                                                driver?.Quit();
                                            }
                                            catch { }
                                            driver = CreateDriver();
                                            driver.Manage().Window.Minimize();
                                        }
                                    }
                                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                                    html = driver.PageSource;
                                    n = html.IndexOf("Перейти на страницу");
                                    if (n >= 0)
                                    {
                                        html = html.Substring(n);
                                        n = html.IndexOf("<input");
                                        if (n >= 0)
                                        {
                                            html = html.Substring(n);
                                            html = html.Substring(html.IndexOf("class=\"") + "class=\"".Length);
                                            html = html.Substring(0, html.IndexOf("\""));
                                            items = driver.FindElementsByCssSelector($"input[class='{html}']");
                                            if (items.Count == 0)
                                                break;
                                            foreach (var item in items)
                                            {
                                                try
                                                {
                                                    if (item.GetAttribute("type") == "number")
                                                    {
                                                        html = "";
                                                        item.Clear();
                                                        item.Click();
                                                        item.SendKeys(page.ToString());
                                                        item.SendKeys(OpenQA.Selenium.Keys.Enter);
                                                        break;
                                                    }
                                                }
                                                catch { }
                                            }
                                            if (html != "")
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                catch
                                { }
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                this.Invoke(new Action(() =>
                                {
                                    MessageBox.Show(ex.Message, "Непредвиденная ошибка (OZON)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }));
                            }
                            catch
                            {
                            }
                        }
                        worksheet.Cells[1, 1, count + 1, ozonFields.Count].AutoFitColumns();
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("По вашему запросу ничего не найдено (OZON)", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }));
                    }
                }
                catch
                {
                }

                try
                {
                    driver?.Close();
                    driver?.Quit();
                }
                catch { }
                this.Invoke(new Action(() =>
                {
                    ozonCountTextBox.Text = int.TryParse(prevCount, out int a) ? prevCount : "Все";
                    ozonCountTextBox.Tag = int.TryParse(prevCount, out a);
                }));
                parsers.Remove(Thread.CurrentThread);
            });
        }

        private string CreateExceptionString(Exception ex)
        {
            return "[" + DateTime.Now.ToString("HH::mm::ss") + "] " + (ex?.Message ?? "") + " ";
        }

        private Thread GetBeruParser(string query, int maxCount, string sort, ExcelPackage package)
        {
            return new Thread(() =>
            {
                ChromeDriver driver = CreateDriver();
                driver.Manage().Window.Minimize();
                string prevCount = beruCountTextBox.Text;
                try
                {
                    string text = "";
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    string request = "";
                    while (true)
                    {
                        try
                        {
                            driver.Navigate().GoToUrl($"https://pokupki.market.yandex.ru/search?cvredirect=2&text={GetFormattedString(query)}");
                            break;
                        }
                        catch
                        {
                            try
                            {
                                driver?.Close();
                                driver?.Quit();
                            }
                            catch { }
                            driver = CreateDriver();
                            driver.Manage().Window.Minimize();
                        }
                    }
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    Thread.Sleep(1000);
                    if (driver.Url.StartsWith("https://beru.ru/showcaptcha"))
                    {
                        SolveCaptcha(driver, "https://beru.ru/showcaptcha");
                    }
                    request = driver.Url + $"&how={sort}";
                    while (true)
                    {
                        try
                        {
                            driver.Navigate().GoToUrl(request);
                            break;
                        }
                        catch
                        {
                            try
                            {
                                driver?.Close();
                                driver?.Quit();
                            }
                            catch { }
                            driver = CreateDriver();
                            driver.Manage().Window.Minimize();
                        }
                    }
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    Thread.Sleep(1000);
                    if (driver.Url.StartsWith("https://beru.ru/showcaptcha"))
                    {
                        SolveCaptcha(driver, "https://beru.ru/showcaptcha");
                    }
                    string html = driver.PageSource.Replace("&nbsp;", "").Replace("&thinsp;", "").Replace(" ", "");
                    int n = html.IndexOf("data-zone-name=\"AdultAlert\"");
                    if (n >= 0)
                    {
                        try
                        {
                            text = html.Substring(n);
                            text = text.Substring(text.IndexOf("<button"));
                            text = text.Substring(text.IndexOf("class=\"") + "class=\"".Length);
                            text = text.Substring(0, text.IndexOf("\""));
                            driver.FindElementByCssSelector($"button[class='{text}']").Click();
                            wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                            Thread.Sleep(100);
                        }
                        catch { }
                    }
                    text = driver.FindElement(By.TagName("html")).Text;
                    n = text.IndexOf("Вы посмотрели");
                    if (n < 0)
                    {
                        n = text.IndexOf("Найден");
                    }
                    if (n >= 0)
                    {
                        text = text.Substring(n);
                        if (text.IndexOf("\r\n") >= 0)
                        {
                            text = text.Substring(0, text.IndexOf("\r\n"));
                        }
                        n = 0;
                        foreach (var t in text.Split(' '))
                        {
                            try
                            {
                                if (int.Parse(t) > n)
                                {
                                    n = int.Parse(t);
                                }
                            }
                            catch { }
                        }
                        if (maxCount < 0 || n < maxCount)
                        {
                            maxCount = n;
                        }
                        if (maxCount > 1200)
                        {
                            maxCount = 1200;
                        }
                        lock (lockObj)
                        {
                            this.maxCount += maxCount;
                        }
                        this.Invoke(new Action(() =>
                        {
                            beruCountTextBox.Text = maxCount.ToString();
                            beruCountTextBox.Tag = true;
                        }));
                        int count = 0;
                        this.Invoke(new Action(() =>
                        {
                            if (maxCount == 0)
                            {
                                beruProgressBar.Value = 100;
                                label9.Text = "100%";
                            }
                            else
                            {
                                beruProgressBar.Value = 0;
                                label9.Text = "0%";
                            }
                        }));

                        Stopwatch stopwatch = new Stopwatch();
                        ReadOnlyCollection<IWebElement> items;
                        List<string> urls = new List<string>();
                        string nextUrl;
                        int page = 1;
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Яндекс.Маркет");
                        foreach (var item in beruFields.Values)
                        {
                            worksheet.SetValue(1, item.Column, item.Name);
                        }
                        string data;
                        ExcelRange cell;
                        int price;
                        try
                        {
                            string data2 = "";
                            while (true)
                            {
                                if (stop)
                                    break;
                                try
                                {
                                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                                    if (driver.Url.StartsWith("https://beru.ru/showcaptcha"))
                                    {
                                        SolveCaptcha(driver, "https://beru.ru/showcaptcha");
                                    }
                                    html = driver.PageSource;
                                    n = html.IndexOf("data-auto=\"SerpPage\"");
                                    data = "f";
                                    if (n >= 0)
                                    {
                                        data = html.Substring(n + "data-auto=\"SerpPage\"".Length);
                                        data = data.Substring(data.IndexOf("<a") + 2);
                                        data = data.Substring(data.IndexOf("class=\"") + "class=\"".Length);
                                        data = data.Substring(0, data.IndexOf("\""));
                                    }
                                    items = driver.FindElementsByCssSelector($"a[class='{data}']");
                                    urls.Clear();
                                    foreach (var item in items)
                                    {
                                        urls.Add(item.GetAttribute("href"));
                                    }
                                    nextUrl = null;

                                    data = driver.PageSource;
                                    n = data.IndexOf("data-auto=\"pagination-next\"");
                                    if (n >= 0)
                                    {
                                        page++;
                                        nextUrl = request + $"&page={page}";
                                    }
                                    foreach (var url in urls)
                                    {
                                        if (stop)
                                            break;
                                        try
                                        {
                                            if (url == null)
                                                continue;
                                            price = -1;
                                            while (true)
                                            {
                                                try
                                                {
                                                    driver.Navigate().GoToUrl(url);
                                                    break;
                                                }
                                                catch
                                                {
                                                    try
                                                    {
                                                        driver?.Close();
                                                        driver?.Quit();
                                                    }
                                                    catch { }
                                                    driver = CreateDriver();
                                                    driver.Manage().Window.Minimize();
                                                }
                                            }
                                            wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                                            if (driver.Url.StartsWith("https://beru.ru/showcaptcha"))
                                            {
                                                SolveCaptcha(driver, "https://beru.ru/showcaptcha");
                                            }
                                            html = "";
                                            stopwatch.Restart();
                                            while (!html.Contains("</div>") && stopwatch.Elapsed.TotalSeconds < 12)
                                            {
                                                html = driver.PageSource.Replace("&nbsp;", " ").Replace("&thinsp;", " ");
                                            }
                                            stopwatch.Stop();
                                            worksheet.SetValue(count + 2, beruFields["query"].Column, query);
                                            text = driver.FindElementByTagName("html").Text;
                                            n = html.IndexOf("data-apiary-widget-name=\"@marketplace/SkuVendorInfo\"");

                                            try
                                            {
                                                data2 = "";
                                                if (n >= 0)
                                                {
                                                    try
                                                    {
                                                        data = html.Substring(n + "data-apiary-widget-name=\"@marketplace/SkuVendorInfo\"".Length);
                                                        n = data.IndexOf("<a");
                                                        data2 = "";
                                                        if (n >= 0)
                                                        {
                                                            data = data.Substring(0, n);
                                                            data = data.Substring(data.IndexOf("<div"));
                                                            data = data.Substring(data.IndexOf("class=\"") + "class=\"".Length);
                                                            foreach (var elem in driver.FindElementsByCssSelector($"div[class='{data.Substring(0, data.IndexOf("\""))}']"))
                                                            {
                                                                try
                                                                {
                                                                    data2 = elem.FindElement(By.TagName("a")).GetAttribute("href");
                                                                    if (data2.Contains("/search") || data2.Contains("/brand/"))
                                                                    {
                                                                        break;
                                                                    }
                                                                    data2 = "";
                                                                }
                                                                catch { }
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        data2 = "";
                                                    }
                                                    try
                                                    {
                                                        data = html;
                                                        n = data.LastIndexOf("<meta itemprop=\"position\" content=\"3\"");
                                                        if (n >= 0)
                                                        {
                                                            data = data.Substring(n);
                                                            data = data.Substring(data.IndexOf("<span") + "<span".Length);
                                                            data = data.Substring(data.IndexOf(">") + 1);
                                                            data = data.Substring(0, data.IndexOf("</span>"));
                                                        }
                                                        else
                                                        {
                                                            n = data.IndexOf("data-apiary-widget-name=\"@marketplace/SkuSupplierOperationalRating\"");
                                                            if (n >= 0)
                                                            {
                                                                data = data.Substring(0, n);
                                                                data = data.Substring(0, data.LastIndexOf("<a"));
                                                                data = data.Substring(data.LastIndexOf("<div"));
                                                                data = data.Substring(data.IndexOf("class=\"") + "class=\"".Length);
                                                                data = data.Substring(0, data.IndexOf("\""));
                                                                items = driver.FindElementsByCssSelector($"div[class='{data}']");
                                                                data = "";
                                                                foreach (var elem in items)
                                                                {
                                                                    foreach (var elem2 in elem.FindElements(By.TagName("a")))
                                                                    {
                                                                        try
                                                                        {
                                                                            data2 = elem2.GetAttribute("href");
                                                                            if (data2.Contains("/supplier/"))
                                                                            {
                                                                                data = elem2.Text;
                                                                                break;
                                                                            }
                                                                        }
                                                                        catch { }
                                                                    }
                                                                    if (data != "")
                                                                    {
                                                                        break;
                                                                    }
                                                                    data2 = "";
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch { }
                                                    try
                                                    {
                                                        if (data2 == "")
                                                        {
                                                            data2 = html;
                                                            n = data2.LastIndexOf("<meta itemprop=\"position\" content=\"3\"");
                                                            if (n >= 0)
                                                            {
                                                                data2 = data2.Substring(n);
                                                                data2 = data2.Substring(data2.IndexOf("<a"));
                                                                data2 = data2.Substring(data2.IndexOf("class=\"") + "class=\"".Length);
                                                                foreach (var elem in driver.FindElementsByCssSelector($"a[class='{data2.Substring(0, data2.IndexOf("\""))}']"))
                                                                {
                                                                    data2 = "";
                                                                    if (elem.GetAttribute("title") == data)
                                                                    {
                                                                        data2 = elem.GetAttribute("href");
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        data2 = "";
                                                    }
                                                    try
                                                    {
                                                        cell = worksheet.Cells[count + 2, beruFields["brand"].Column];
                                                        if (data != "" && data2 != "")
                                                        {
                                                            cell.Hyperlink = new ExcelHyperLink(data2, UriKind.Absolute) { Display = data };
                                                        }
                                                        else
                                                        {
                                                            cell.Value = data;
                                                        }

                                                    }
                                                    catch
                                                    { }
                                                }
                                            }
                                            catch { }
                                            n = html.IndexOf("data-zone-name=\"summary\"");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = html.Substring(n);
                                                    data = data.Substring(data.IndexOf("<h1"));
                                                    data = data.Substring(data.IndexOf(">") + 1);
                                                    data = data.Substring(0, data.IndexOf("</h1>"));
                                                    cell = worksheet.Cells[count + 2, beruFields["name"].Column];
                                                    cell.Hyperlink = new ExcelHyperLink(url, UriKind.Absolute) { Display = data };
                                                }
                                                catch { }
                                            }
                                            try
                                            {
                                                foreach (var code in url.Replace("?", "/").Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
                                                {
                                                    if (IsNumber(code))
                                                    {
                                                        try
                                                        {
                                                            worksheet.SetValue(count + 2, beruFields["article"].Column, ulong.Parse(code));
                                                        }
                                                        catch
                                                        {
                                                            worksheet.SetValue(count + 2, beruFields["article"].Column, code);
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                            catch { }
                                            data = html;
                                            n = data.IndexOf("data-auto=\"price\"");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = data.Substring(n);
                                                    data = data.Substring(0, data.IndexOf("</span>"));
                                                    data = data.Substring(data.LastIndexOf(">") + 1);
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (!char.IsDigit(data[i]))
                                                        {
                                                            data = data.Remove(i, 1);
                                                            i--;
                                                        }
                                                    }
                                                    try
                                                    {
                                                        price = int.Parse(data);
                                                        worksheet.SetValue(count + 2, beruFields["priceafter"].Column, price);
                                                    }
                                                    catch { }
                                                }
                                                catch { }
                                            }
                                            data = html;
                                            n = data.IndexOf("data-auto=\"old-price\"");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = data.Substring(n);
                                                    data = data.Substring(0, data.IndexOf("</span>"));
                                                    data = data.Substring(data.LastIndexOf(">") + 1);
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (!char.IsDigit(data[i]))
                                                        {
                                                            data = data.Remove(i, 1);
                                                            i--;
                                                        }
                                                    }
                                                    try
                                                    {
                                                        price = int.Parse(data);
                                                        worksheet.SetValue(count + 2, beruFields["pricebefore"].Column, price);
                                                    }
                                                    catch { }
                                                }
                                                catch { }
                                            }
                                            data = text;
                                            data2 = "Вес\r\n";
                                            n = data.LastIndexOf("Вес\r\n");
                                            if (n < 0)
                                            {
                                                n = data.IndexOf("Вес упаковки\r\n");
                                                data2 = "Вес упаковки\r\n";
                                            }
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = data.Substring(n + data2.Length);
                                                    try
                                                    {
                                                        data = data.Substring(0, data.IndexOf("\r\n"));
                                                    }
                                                    catch { }
                                                    data2 = "г";
                                                    if (data.Contains("кг"))
                                                        data2 = "кг";
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (!char.IsDigit(data[i]) && data[i] != '.')
                                                        {
                                                            data = data.Remove(i, 1);
                                                            i--;
                                                        }
                                                    }
                                                    worksheet.SetValue(count + 2, beruFields["weight"].Column, (int)(double.Parse(data, CultureInfo.InvariantCulture) * (data2 == "кг" ? 1000 : 1)));
                                                    if (price > 0)
                                                    {
                                                        try
                                                        {
                                                            worksheet.SetValue(count + 2, beruFields["price"].Column, price * 100 / (int)(double.Parse(data, CultureInfo.InvariantCulture) * (data2 == "кг" ? 1000 : 1)));
                                                        }
                                                        catch
                                                        { }
                                                    }
                                                }
                                                catch { }
                                            }
                                            else
                                            {
                                                n = data.IndexOf("Вес упаковки:");
                                                if (n < 0)
                                                {
                                                    n = data.IndexOf("Вес: ");
                                                }
                                                if (n >= 0)
                                                {
                                                    data = data.Substring(n);
                                                    data = data.Substring(0, data.IndexOf("\r\n"));
                                                    data2 = "г";
                                                    if (data.Contains("кг"))
                                                        data2 = "кг";
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (!char.IsDigit(data[i]) && data[i] != '.')
                                                        {
                                                            data = data.Remove(i, 1);
                                                            i--;
                                                        }
                                                    }
                                                    worksheet.SetValue(count + 2, beruFields["weight"].Column, (int)(double.Parse(data, CultureInfo.InvariantCulture) * (data2 == "кг" ? 1000 : 1)));
                                                    if (price > 0)
                                                    {
                                                        try
                                                        {
                                                            worksheet.SetValue(count + 2, beruFields["price"].Column, price * 100 / (int)(double.Parse(data, CultureInfo.InvariantCulture) * (data2 == "кг" ? 1000 : 1)));
                                                        }
                                                        catch
                                                        { }
                                                    }
                                                }
                                            }
                                            data = text;
                                            n = data.IndexOf(" отзыв");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = data.Substring(0, n);
                                                    data = data.Substring(data.LastIndexOf("\r\n") + 2);
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (!char.IsDigit(data[i]))
                                                        {
                                                            data = data.Remove(i, 1);
                                                            i--;
                                                        }
                                                    }
                                                    if (data.Length > 0)
                                                    {
                                                        worksheet.SetValue(count + 2, beruFields["comments"].Column, int.Parse(data));
                                                    }
                                                    else
                                                    {
                                                        worksheet.SetValue(count + 2, beruFields["comments"].Column, 0);
                                                    }
                                                }
                                                catch { }
                                            }
                                            else
                                            {
                                                worksheet.SetValue(count + 2, beruFields["comments"].Column, 0);
                                            }
                                            data = html;
                                            n = data.IndexOf("data-zone-name=\"reasonsToBuy\"");
                                            if (n >= 0)
                                            {
                                                try
                                                {
                                                    data = data.Substring(n);
                                                    try
                                                    {
                                                        data = data.Substring(0, data.IndexOf("</span>"));
                                                    }
                                                    catch { }
                                                    data = data.Substring(data.LastIndexOf(">") + 1);
                                                    data2 = "";
                                                    for (int i = 0; i < data.Length; i++)
                                                    {
                                                        if (char.IsDigit(data[i]))
                                                        {
                                                            data2 += data[i];
                                                        }
                                                        else
                                                        {
                                                            if (data2 != "")
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    if (data.Contains("интерес"))
                                                    {
                                                        worksheet.SetValue(count + 2, beruFields["interest"].Column, int.Parse(data2));
                                                    }
                                                    else
                                                    {
                                                        worksheet.SetValue(count + 2, beruFields["sales"].Column, int.Parse(data2));
                                                    }
                                                }
                                                catch { }
                                            }
                                        }
                                        catch { }
                                        count++;
                                        lock (lockObj)
                                        {
                                            this.count++;
                                        }
                                        this.Invoke(new Action(() =>
                                        {
                                            beruProgressBar.Value = 100 * count / maxCount;
                                            label9.Text = Math.Round(100.0 * count / maxCount, 1).ToString().Replace(",", ".") + "%";
                                        }));

                                        if (count == maxCount)
                                            break;
                                    }
                                    if (count == maxCount)
                                        break;
                                    if (nextUrl == null)
                                        break;
                                    while (true)
                                    {
                                        try
                                        {
                                            driver.Navigate().GoToUrl(nextUrl);
                                            break;
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                driver?.Close();
                                                driver?.Quit();
                                            }
                                            catch { }
                                            driver = CreateDriver();
                                            driver.Manage().Window.Minimize();
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                this.Invoke(new Action(() =>
                                {
                                    MessageBox.Show(ex.Message, "Непредвиденная ошибка (Яндекс.Маркет)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }));
                            }
                            catch { }
                        }
                        worksheet.Cells[1, 1, count + 1, beruFields.Count].AutoFitColumns();
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("По вашему запросу ничего не найдено (Яндекс.Маркет)", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }));
                    }
                }
                catch { }
                try
                {
                    driver?.Close();
                    driver?.Quit();
                }
                catch { }
                this.Invoke(new Action(() =>
                {
                    beruCountTextBox.Text = int.TryParse(prevCount, out int a) ? prevCount : "Все";
                    beruCountTextBox.Tag = int.TryParse(prevCount, out a);
                }));
                parsers.Remove(Thread.CurrentThread);
            });
        }

        private bool IsNumber(string s)
        {
            if (s == null || s.Length == 0)
                return false;
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsDigit(s[i]))
                    return false;
            }
            return true;
        }

        private void SolveCaptcha(ChromeDriver driver, string start)
        {
            string captcha;
            CaptchaForm captchaForm;
            IWebElement textBox;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            while (driver.Url.StartsWith(start))
            {
                try
                {
                    do
                    {
                        captcha = driver.FindElementByClassName("captcha__image").FindElement(By.TagName("img")).GetAttribute("src");
                        captchaForm = new CaptchaForm(captcha);
                    }
                    while (captchaForm.ShowDialog() != DialogResult.OK);
                    textBox = driver.FindElementByClassName("input-wrapper__content");
                    textBox.Clear();
                    textBox.Click();
                    textBox.SendKeys(captchaForm.CaptchaText);
                    driver.FindElementByClassName("submit").Click();
                    wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                }
                catch { }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                foreach (var thread in parsers)
                {
                    if (thread.IsAlive)
                    {
                        var res = MessageBox.Show("Парсинг ещё не окончен.\nВы точно хоите прервать работу программы?", "Опасность", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                        if (res != DialogResult.Yes)
                        {
                            e.Cancel = true;
                            return;
                        }
                        break;
                    }
                }
                foreach (var thread in parsers)
                {
                    thread?.Abort();
                }
            }
            catch
            {
            }
            commonStop = true;
            if (!saved)
            {
                try
                {
                    if (package != null && package.Workbook.Worksheets.Count > 0)
                    {
                        if (!Directory.Exists("outFiles"))
                            Directory.CreateDirectory("outFiles");
                        string path = Path.Combine("outFiles", "Autosave " + DateTime.Now.ToString("MM_dd_yyyy HH-mm-ss") + ".xlsx");
                        using (File.Create(path))
                        { }
                        package?.SaveAs(new FileInfo(path));
                    }
                }
                catch { }
            }
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.ProcessName == "chromedriver" || process.ProcessName == "chrome")
                    {
                        process.Kill();
                    }
                }
                catch { }
            }
        }

        private void queryTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter && searchButton.Text == "Поиск")
            {
                e.SuppressKeyPress = true;
                button1_Click(null, null);
            }
        }

        private void tableLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            SetControlPosition(WBActiveCheckBox);
            SetControlPosition(ozonActiveCheckBox);
            SetControlPosition(beruActiveCheckBox);
            SetControlPosition(WBRemainsCheckBox);
            SetControlPosition(ozonRemainsCheckBox);
            SetControlPosition(beruRemainsCheckBox);
        }

        private void SetControlPosition(Control control)
        {
            control.Left = (control.Parent.Width - control.Width) / 2;
            control.Top = (control.Parent.Height - control.Height) / 2;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                foreach (var child in ((Control)(sender)).Controls)
                {
                    if (child is CheckBox checkBox)
                    {
                        checkBox.Checked = !checkBox.Checked;
                        checkBox.Focus();
                    }
                }
            }
            catch { }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            if (commonProgressBar.Visible)
            {
                e.Graphics.FillRectangle(new SolidBrush(panel7.BackColor), panel7.Left - 2, panel7.Top - 2, panel7.Width + 4, panel7.Height + 4);
            }
        }

        private void WBPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    Process.Start("https://www.wildberries.ru");
                }
            }
            catch { }
        }

        private void ozonPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    Process.Start("https://www.ozon.ru");
                }
            }
            catch { }
        }

        private void beruPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    Process.Start("https://pokupki.market.yandex.ru/");
                }
            }
            catch { }
        }

        private void SetProgress(Panel panel, ProgressBar progressBar, Label label)
        {
            progressBar.Left = 0;
            progressBar.Height = panel.Height;
            progressBar.Width = panel.Width - label.Width;
            progressBar.Top = 0;
            label.Left = progressBar.Right;
            label.Top = (panel.Height - label.Height) / 2;
        }

        private void panel7_SizeChanged(object sender, EventArgs e)
        {
            SetProgress(panel7, commonProgressBar, label10);
        }

        private void panel6_SizeChanged(object sender, EventArgs e)
        {
            SetProgress(panel6, beruProgressBar, label9);
        }

        private void panel5_SizeChanged(object sender, EventArgs e)
        {
            SetProgress(panel5, ozonProgressBar, label8);
        }

        private void panel4_SizeChanged(object sender, EventArgs e)
        {
            SetProgress(panel4, WBProgressBar, label7);
        }
    }
}