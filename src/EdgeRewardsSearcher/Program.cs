using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;

namespace EdgeRewardsSearcher
{
    internal class Program
    {
        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int nIndex);

        const int SM_CXSCREEN = 0;
        const int SM_CYSCREEN = 1;

        static void Main(string[] args)
        {
            int screenWidth = GetSystemMetrics(SM_CXSCREEN);
            int screenHeight = GetSystemMetrics(SM_CYSCREEN);

            Console.WriteLine($"当前屏幕分辨率(Screen Resolution): {screenWidth} x {screenHeight}");

            Console.Write("请输入搜索框 X 坐标 (Enter search box X coordinate):");
            double inputX = double.Parse(Console.ReadLine() ?? "0");

            Console.Write("请输入搜索框 Y 坐标 (Enter search box Y coordinate):");
            double inputY = double.Parse(Console.ReadLine() ?? "0");

            Console.Write("请输入循环次数 (Enter number of loops): ");
            int loopCount = int.Parse(Console.ReadLine() ?? "1");

            Console.Write("请输入点击间隔(毫秒)，默认2000 (Enter click interval in ms, default = 2000): ");
            string intervalInput = Console.ReadLine();
            int clickInterval = string.IsNullOrWhiteSpace(intervalInput) ? 2000 : int.Parse(intervalInput);

            Console.WriteLine("按任意键开始 (Press any key to start)...");
            Console.ReadKey();
            Thread.Sleep(2000);

            var sim = new InputSimulator();
            for (int i = 0; i < loopCount; i++) {
                double virtualX = inputX * 65535 / screenWidth;
                double virtualY = inputY * 65535 / screenHeight;
                sim.Mouse.MoveMouseTo(virtualX, virtualY);
                Thread.Sleep(100);
                sim.Mouse.LeftButtonClick();
                Thread.Sleep(300);
                string keyword = GetRandomContent();
                Console.WriteLine($"输入内容(Input content):{keyword}");
                sim.Keyboard.TextEntry(keyword);
                Thread.Sleep(500);
                sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                Thread.Sleep(clickInterval);
            }

            Console.WriteLine("任务结束，按任意键退出(Task finished, press any key to exit)...");
            Console.ReadKey();
        }

        private static Random rnd = new Random();

        //主语
        private static List<string> subjects = new List<string>
        {
            "我", "你", "他", "她", "我们", "他们", "某人", "大家",
            "I", "You", "He", "She", "We", "They", "Someone"
        };

        // 动词
        private static List<string> verbs = new List<string>
        {
            "喜欢", "讨厌", "爱", "看见", "听到", "学习", "写", "吃", "玩", "跑", "想念",
            "like", "hate", "love", "see", "hear", "study", "write", "eat", "play", "miss"
        };

        // 宾语
        private static List<string> objects = new List<string>
        {
            "苹果", "书", "电影", "音乐", "游戏", "猫", "朋友", "代码", "咖啡", "旅行",
            "apple", "book", "movie", "music", "game", "cat", "friend", "code", "coffee", "travel"
        };

        // 副词
        private static List<string> adverbs = new List<string>
        {
            "每天", "经常", "偶尔", "很快", "慢慢地", "认真地", "突然", "安静地",
            "every day", "often", "sometimes", "quickly", "slowly", "suddenly", "quietly"
        };

        // 单字
        private static List<string> singleChars = new List<string>
        {
            "爱", "书", "猫", "乐", "快", "学", "看", "听",
            "A", "B", "C", "X", "Z"
        };

        // 单词
        private static List<string> words = new List<string>
        {
            "苹果", "电影", "游戏", "音乐", "学校", "朋友", "写作", "睡觉", "梦想",
            "apple", "game", "school", "friend", "dream", "coding", "coffee", "sleep"
        };

        // 句子模式
        private static List<Func<string>> sentencePatterns = new List<Func<string>>
        {
            () => subjects[rnd.Next(subjects.Count)] + verbs[rnd.Next(verbs.Count)] + objects[rnd.Next(objects.Count)],
            () => subjects[rnd.Next(subjects.Count)] + verbs[rnd.Next(verbs.Count)] + objects[rnd.Next(objects.Count)] + adverbs[rnd.Next(adverbs.Count)],
            () => subjects[rnd.Next(subjects.Count)] + " and " + subjects[rnd.Next(subjects.Count)] + " " + verbs[rnd.Next(verbs.Count)] + objects[rnd.Next(objects.Count)],
            () => "Sometimes " + subjects[rnd.Next(subjects.Count)] + " " + verbs[rnd.Next(verbs.Count)] + " " + objects[rnd.Next(objects.Count)]
        };

        // 方法：返回一个随机内容
        public static string GetRandomContent()
        {
            int mode = rnd.Next(3); // 0=单字, 1=单词, 2=短语
            string phrase = "";

            if (mode == 0) {
                phrase = singleChars[rnd.Next(singleChars.Count)];
            }
            else if (mode == 1) {
                phrase = words[rnd.Next(words.Count)];
            }
            else {
                var pattern = sentencePatterns[rnd.Next(sentencePatterns.Count)];
                phrase = pattern();
            }

            return phrase;
        }

    }
}
