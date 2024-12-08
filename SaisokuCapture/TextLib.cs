//2024.05.26
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TextLib
{
    /// <summary>
    /// iniファイル読み書き用クラス
    /// </summary>
    public class IniFile
    {
        //iniファイルパス
        private string _iniFilepath;

        //iniファイルエンコーディング
        private Encoding _encoding = null;

        //セクションとキーの集合
        private Items _items = new Items();

        //デフォルトのiniファイルパス
        private string _baseIniFilepath
        {
            get
            {
                string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string directory = Path.GetDirectoryName(filepath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filepath);
                return directory + @"\" + fileNameWithoutExtension + ".ini";
            }
        }
        /// <summary>
        /// iniファイルの初期化。App.exeの場合App.iniをiniファイルとします。Encodingは自動で判定します。
        /// </summary>
        public IniFile()
        {
            _iniFilepath = _baseIniFilepath;
            _encoding = (_encoding ?? EncodeLib.GetJpEncoding(_iniFilepath)) ?? EncodeLib.UTF8;
        }

        /// <summary>
        /// iniファイルの初期化。iniファイルの絶対パスを指定します。Encodingは自動で判定します。
        /// </summary>
        /// <param name="filepath">iniファイルの絶対パス</param>
        public IniFile(string filepath)
        {
            _iniFilepath = filepath;
            _encoding = (_encoding ?? EncodeLib.GetJpEncoding(_iniFilepath)) ?? EncodeLib.UTF8;
        }

        /// <summary>
        /// iniファイルの初期化。iniファイルの絶対パスとEncodingを指定します。
        /// </summary>
        /// <param name="filepath">iniファイルの絶対パス</param>
        /// <param name="encoding">iniファイルの文字コード</param>
        public IniFile(string filepath, Encoding encoding)
        {
            _iniFilepath = filepath;
            _encoding = encoding;
        }

        /// <summary>
        /// iniファイルの初期化。iniファイルのEncodingを指定します。iniファイルはApp.exeの場合App.iniとします。
        /// </summary>
        /// <param name="encoding">iniファイルの文字コード</param>
        public IniFile(Encoding encoding)
        {
            _iniFilepath = _baseIniFilepath;
            _encoding = encoding;
        }

        /// <summary>
        /// キーの値の取得
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <returns>キーが存在する場合はその値（文字列）、存在しない場合はnull</returns>
        public string GetKeyValue(string sectionName, string keyName)
        {
            //ファイル読み込み
            LoadIniFile();

            //文字列取得
            return _items.GetSection(sectionName)?.GetKey(keyName)?.Value;
        }

        /// <summary>
        /// キーの値の取得
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="save">キーが存在しない場合にキーを作成するかどうか。空文字列を値として作成します。</param>
        /// <returns>キーが存在する場合はその値（文字列）、存在しない場合は空文字列</returns>
        public string GetKeyValueString(string sectionName, string keyName, bool save = false)
        {
            string value = GetKeyValue(sectionName, keyName);

            //存在しない場合
            if (value == null && save)
            {
                SetKeyValueString(sectionName, keyName, "");
            }

            return value ?? "";
        }

        /// <summary>
        /// キーの値の取得
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="defaultValue">キーが存在しないか空文字列の場合のデフォルト値</param>
        /// <param name="save">キーが存在しないか空文字列の場合にキーを作成するかどうか。defaultValueを値として作成します。</param>
        /// <returns>キーが存在し値が空文字列でない場合はその値、それ以外はdefaultValue</returns>
        public string GetKeyValueStringWithoutEmpty(string sectionName, string keyName, string defaultValue, bool save = false)
        {
            string value = GetKeyValue(sectionName, keyName);

            //存在しない場合
            if (value != null && value != "")
            {
                return value;
            }
            else
            {
                if (save)
                {
                    SetKeyValueString(sectionName, keyName, defaultValue);
                }
                return defaultValue;
            }
        }

        /// <summary>
        /// キーの値の取得
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="defaultValue">キーが存在しないか空文字列の場合のデフォルト値</param>
        /// <param name="save">キーが存在しないか値が1でも0でもない場合にキーを作成するかどうか。defaultValueを値として作成します。</param>
        /// <returns>キーが存在し値が1ならtrue、0ならfalse、それ以外はdefaultValue</returns>
        public bool GetKeyValueBool(string sectionName, string keyName, bool defaultValue, bool save = false)
        {
            string value = GetKeyValue(sectionName, keyName);

            if (value != null)
            {
                if (value == "1")
                {
                    return true;
                }
                else if (value == "0")
                {
                    return false;
                }
            }

            if (save)
            {
                SetKeyValueBool(sectionName, keyName, defaultValue);
            }

            return defaultValue;
        }

        /// <summary>
        /// キーの値の取得
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="defaultValue">キーが存在しないか数値変換できない場合のデフォルト値</param>
        /// <param name="save">キーが存在しないか数値変換できない場合にキーを作成するかどうか。defaultValueを値として作成します。</param>
        /// <returns>キーが存在し数値変換できる場合はその値、それ以外はdefaultValue</returns>
        public int GetKeyValueInt(string sectionName, string keyName, int defaultValue, bool save = false)
        {
            string value = GetKeyValue(sectionName, keyName);

            int ret;
            if (value != null && int.TryParse(value, out ret))
            {
                return ret;
            }

            if (save)
            {
                SetKeyValueInt(sectionName, keyName, defaultValue);
            }

            return defaultValue;
        }

        /// <summary>
        /// キーの値の取得
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="defaultValue">キーが存在しないか数値変換できない場合のデフォルト値</param>
        /// <param name="vmin">キーの値として許容する範囲の下限値</param>
        /// <param name="vmax">キーの値として許容する範囲の上限値</param>
        /// <param name="save">キーが存在しない、数値変換できない、範囲が適切でない、のいずれか場合にキーを作成または値の修正をするかどうか。</param>
        /// <returns>キーが存在し数値変換できる場合はその値。それ以外はdefaultValue。但し範囲が不適切なら修正後の値とする。</returns>
        public int GetKeyValueInt(string sectionName, string keyName, int defaultValue, int vmin, int vmax, bool save = false)
        {
            string value = GetKeyValue(sectionName, keyName);

            int ret;
            if (value != null && int.TryParse(value, out ret))
            {
                if (vmin <= ret && ret <= vmax)
                {
                    return ret;
                }
            }
            else
            {
                ret = defaultValue;
            }

            if (ret < vmin)
            {
                ret = vmin;
            }
            if (vmax < ret)
            {
                ret = vmax;
            }

            if (save)
            {
                SetKeyValueInt(sectionName, keyName, ret);
            }

            return ret;
        }

        /// <summary>
        /// キーの値を配列で取得
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <returns>全てのセクション名、キー名、値を含む配列。セクション名を指定した場合はそのセクション配下のキーのみ取得</returns>
        public (string, string, string)[] GetKeyValueAsArray(string sectionName = "")
        {
            //ファイル読み込み
            LoadIniFile();

            List<(string, string, string)> datas = new List<(string, string, string)>();

            if (sectionName == "")
            {
                foreach (Section section in _items.Sections)
                {
                    foreach (Key key in section.Keys)
                    {
                        datas.Add((section.Name, key.Name, key.Value));
                    }
                }
            }
            else
            {
                Section section = _items.GetSection(sectionName);
                if (section != null)
                {
                    foreach (Key key in section.Keys)
                    {
                        datas.Add((sectionName, key.Name, key.Value));
                    }
                }
            }

            return datas.ToArray();
        }

        private void SetValue(string sectionName, string keyName, string value)
        {
            Section section = _items.GetSection(sectionName);

            if (section == null)
            {
                section = new Section(sectionName);
                _items.Sections.Add(section);
            }

            Key key = section.GetKey(keyName);

            if (key == null)
            {
                section.Keys.Add(new Key(keyName, value));
            }
            else
            {
                key.Value = value;
            }
        }

        /// <summary>
        /// キーの値（文字列）の設定
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="value">キーの値</param>
        public void SetKeyValueString(string sectionName, string keyName, string value)
        {
            //ファイル読み込み
            LoadIniFile();

            //値の設定
            SetValue(sectionName, keyName, value);

            //ファイル保存
            WriteIniFile();
        }

        /// <summary>
        /// キーの値（数値）の設定
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="value">キーの値</param>
        public void SetKeyValueInt(string sectionName, string keyName, int value)
        {
            SetKeyValueString(sectionName, keyName, value.ToString());
        }

        /// <summary>
        /// キーの値（真偽）の設定。trueの場合1、falseの場合0が設定される。
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="value">キーの値</param>
        public void SetKeyValueBool(string sectionName, string keyName, bool value)
        {
            SetKeyValueInt(sectionName, keyName, value ? 1 : 0);
        }

        /// <summary>
        /// キーの値を配列で一括設定
        /// </summary>
        /// <param name="datas">セクション名、キー名、キーの値の配列。キーの値はstring、int、boolのいずれか</param>
        public void SetKeyValueFromArray((string, string, object)[] datas)
        {
            LoadIniFile();

            foreach ((string sectionName, string keyName, object value) in datas)
            {
                if (value.GetType() == typeof(int))//int
                {
                    SetValue(sectionName, keyName, ((int)value).ToString());
                }
                else if (value.GetType() == typeof(string))//string
                {
                    SetValue(sectionName, keyName, (string)value);
                }
                else if (value.GetType() == typeof(bool))//bool
                {
                    SetValue(sectionName, keyName, (bool)value ? "1" : "0");
                }
            }

            WriteIniFile();
        }

        /// <summary>
        /// キーの値の削除。キーの値を空文字列にします。
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        public void DeleteKeyValue(string sectionName, string keyName)
        {
            //ファイル読み込み
            LoadIniFile();

            Key key = _items.GetSection(sectionName)?.GetKey(keyName);

            if (key != null)
            {
                key.Value = "";
            }

            //ファイル保存
            WriteIniFile();
        }

        /// <summary>
        /// キーの削除
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        public void DeleteKey(string sectionName, string keyName)
        {
            //ファイル読み込み
            LoadIniFile();

            //キー削除
            Section section = _items.GetSection(sectionName);
            section?.Keys.Remove(section?.GetKey(keyName));

            //ファイル保存
            WriteIniFile();
        }

        /// <summary>
        /// セクションの削除
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        public void DeleteSection(string sectionName)
        {
            //ファイル読み込み
            LoadIniFile();

            //セクション削除
            _items.Sections.Remove(_items.GetSection(sectionName));

            //ファイル保存
            WriteIniFile();
        }

        //セクションとキー
        //全項目
        private class Items
        {
            public List<Section> Sections { get; set; }

            public Items()
            {
                Sections = new List<Section>();
            }

            //指定のセクション名のセクションを返す
            public Section GetSection(string sectionName) => Sections.Find(section => section.Name == sectionName);

            //全項目設定文字列を返す
            public string ToStr()
            {
                StringBuilder sb = new StringBuilder();

                foreach (Section section in Sections)
                {
                    if (section.Keys.Count != 0)
                    {
                        sb.Append("[");
                        sb.Append(section.Name);
                        sb.AppendLine("]");

                        foreach (Key key in section.Keys)
                        {
                            sb.Append(key.Name);
                            sb.Append("=");
                            sb.AppendLine(key.Value);
                        }
                    }
                }

                return sb.ToString();
            }
        }

        //セクション
        private class Section
        {
            public string Name { get; set; }
            public List<Key> Keys { get; set; }

            public Section(string name)
            {
                Name = name;
                Keys = new List<Key>();
            }

            //指定のキー名のキーを返す
            public Key GetKey(string keyName) => Keys.Find(key => key.Name == keyName);
        }

        //キー
        private class Key
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public Key(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }

        //iniファイルの読み込み
        private bool LoadIniFile()
        {
            //初期化
            _items = new Items();

            //対象ファイルが設定されていない場合
            if (_iniFilepath == "")
            {
                return false;
            }

            //対象ファイルが存在しない場合は新規作成
            if (!File.Exists(_iniFilepath))
            {
                try
                {
                    File.Create(_iniFilepath).Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            //ファイルが存在する場合
            if (File.Exists(_iniFilepath))
            {
                //空ファイルでなければ
                if (new FileInfo(_iniFilepath).Length != 0)
                {
                    //ファイル読み込み
                    string allText = TextFile.Read(_iniFilepath, _encoding);
                    if (allText == null)
                    {
                        return false;
                    }

                    //中身確認
                    if (allText != "")
                    {
                        //改行コードで切り分けて配列に格納
                        string[] lines = allText.Replace("\r\n", "\n").Split('\n');

                        //読み込んだ内容を格納
                        bool isSectionExist = false;
                        foreach (string line in lines)
                        {
                            if (line.Length > 0)
                            {
                                //section
                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    isSectionExist = false;
                                    if (line.Length >= 3)//何らかの中身があるはずで
                                    {
                                        //取得
                                        string sectionName = line.Substring(1, line.Length - 2).Trim();

                                        //セクション名が空白でなく、かつ重複していないとき
                                        if (sectionName.Length != 0 && _items.GetSection(sectionName) == null)
                                        {
                                            //確保
                                            _items.Sections.Add(new Section(sectionName));
                                            isSectionExist = true;
                                        }
                                    }
                                }
                                //key
                                else if (line.IndexOf('=') >= 1)
                                {
                                    if (isSectionExist)
                                    {
                                        //取得
                                        int index = line.IndexOf('=');
                                        string name = line.Substring(0, index);
                                        string value = line.Substring(index + 1);

                                        //現在のsectionにキー追加
                                        _items.Sections[_items.Sections.Count - 1].Keys.Add(new Key(name, value));
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        //iniファイルの書き込み
        private bool WriteIniFile()
        {
            return TextFile.Write(_iniFilepath, _items.ToStr(), false, _encoding);
        }
    }

    public class TextFile
    {
        /// <summary>
        /// テキストファイルへ文字列を書き込みます。上書き保存します。
        /// </summary>
        /// <param name="file">テキストファイルのフルパス</param>
        /// <param name="allText">書き込む文字列</param>
        /// <returns>正常に書き込みできた場合はtrue、それ以外はfalse</returns>
        public static bool Write(string file, string allText)
        {
            return Write(file, allText, false, EncodeLib.GetJpEncoding(file) ?? EncodeLib.UTF8);
        }

        /// <summary>
        /// テキストファイルへ文字列を書き込みます。上書き保存します。
        /// </summary>
        /// <param name="file">テキストファイルのフルパス</param>
        /// <param name="allText">書き込む文字列</param>
        /// <returns>正常に書き込みできた場合はtrue、それ以外はfalse</returns>
        public static bool WriteOver(string file, string allText)
        {
            return Write(file, allText, false, EncodeLib.GetJpEncoding(file) ?? EncodeLib.UTF8);
        }

        /// <summary>
        /// テキストファイルへ文字列を書き込みます。追加書き込みします。
        /// </summary>
        /// <param name="file">テキストファイルのフルパス</param>
        /// <param name="allText">書き込む文字列</param>
        /// <returns>正常に書き込みできた場合はtrue、それ以外はfalse</returns>
        public static bool WriteAppend(string file, string allText)
        {
            return Write(file, allText, true, EncodeLib.GetJpEncoding(file) ?? EncodeLib.UTF8);
        }

        /// <summary>
        /// テキストファイルへ文字列を書き込みます。
        /// </summary>
        /// <param name="file">テキストファイルのフルパス</param>
        /// <param name="allText">書き込む文字列</param>
        /// <param name="add">追加書き込みの場合はtrue、上書きの場合はfalse</param>
        /// <param name="encoding">テキストファイルのエンコーディング</param>
        /// <returns>正常に書き込みできた場合はtrue、それ以外はfalse</returns>
        public static bool Write(string file, string allText, bool add, Encoding encoding)
        {
            bool check = false;
            StreamWriter streamWriter = null;

            try
            {
                //ファイルを作成
                streamWriter = new StreamWriter(file, add, encoding);
                streamWriter.Write(allText);
                check = true;
            }
            catch (Exception)
            {
                check = false;
            }
            finally
            {
                streamWriter?.Close();
            }

            return check;
        }

        /// <summary>
        /// テキストファイルのすべてのテキストを読み込みます。
        /// </summary>
        /// <param name="file">テキストファイルのフルパス</param>
        /// <returns>読み込んだ全てのテキスト。正常に読み込めなかった場合はnull</returns>
        public static string Read(string file)
        {
            return Read(file, EncodeLib.GetJpEncoding(file) ?? EncodeLib.UTF8);
        }

        /// <summary>
        /// テキストファイルのすべてのテキストを読み込みます。
        /// </summary>
        /// <param name="file">テキストファイルのフルパス</param>
        /// <param name="encoding">テキストファイルのエンコーディング</param>
        /// <returns>読み込んだ全てのテキスト。正常に読み込めなかった場合はnull</returns>
        public static string Read(string file, Encoding encoding)
        {
            string allText = null;
            StreamReader streamReader = null;

            try
            {
                if (File.Exists(file))
                {
                    if (new FileInfo(file).Length != 0)
                    {
                        using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))//読み取り専用で開く
                        {
                            streamReader = new StreamReader(fileStream, encoding);
                            allText = streamReader.ReadToEnd();
                        }
                    }
                    else
                    {
                        allText = "";
                    }
                }
            }
            catch (Exception)
            {
                allText = null;
            }
            finally
            {
                streamReader?.Close();
            }

            return allText;
        }
    }

    public static class EncodeLib
    {
        public static Encoding SJIS => Encoding.GetEncoding(932);
        public static Encoding UTF8withBOM => new UTF8Encoding(true);
        public static Encoding UTF8 => new UTF8Encoding(false);

        /// <summary>
        /// テキストファイルの文字コードを変更します。
        /// </summary>
        /// <param name="file">テキストファイルのフルパス</param>
        /// <param name="encodingNext">変更後の文字コード</param>
        /// <returns>正常に変更できた場合はtrue、それ以外はfalse</returns>
        public static bool ChangeEncode(string file, Encoding encodingNext)
        {
            try
            {
                if (!File.Exists(file))//ファイルが存在しない場合
                {
                    return true;
                }
                else if (new FileInfo(file).Length == 0)//ファイルサイズが0の場合
                {
                    return true;
                }
                else
                {
                    //文字コード確認
                    Encoding encodingCurrent = GetJpEncoding(file);

                    if (encodingCurrent == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (encodingCurrent != encodingNext)
                        {
                            //読み込み
                            string allText = TextFile.Read(file, encodingCurrent);

                            if (allText == null)
                            {
                                return false;
                            }
                            else
                            {
                                TextFile.Write(file, allText, false, encodingNext);
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// テキストファイルの文字コードを取得します。
        /// </summary>
        /// <param name="file">テキストファイルのフルパス</param>
        /// <param name="maxSize">先頭何バイトで文字コードの判定をするか</param>
        /// <returns>判定した文字コード、判定できなかった場合はnull</returns>
        public static Encoding GetJpEncoding(string file, long maxSize = 50 * 1024)
        {
            try
            {
                if (!File.Exists(file))//ファイルが存在しない場合
                {
                    return null;
                }
                else if (new FileInfo(file).Length == 0)//ファイルサイズが0の場合
                {
                    return null;
                }
                else//ファイルが存在しファイルサイズが0でない場合
                {
                    //バイナリ読み込み
                    byte[] bytes = null;
                    bool readAll = false;
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        long size = fs.Length;

                        if (size <= maxSize)
                        {
                            bytes = new byte[size];
                            fs.Read(bytes, 0, (int)size);
                            readAll = true;
                        }
                        else
                        {
                            bytes = new byte[maxSize];
                            fs.Read(bytes, 0, (int)maxSize);
                        }
                    }

                    //判定
                    return GetJpEncoding(bytes, readAll);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// バイト配列の文字コードを取得します。文章の先頭からのバイト配列である（途中からのバイト配列でない）必要があります。
        /// </summary>
        /// <param name="bytes">文字コードを判定するバイト配列</param>
        /// <param name="readAll">バイト配列が文章の一部分のみの場合はfalse、全ての文章の場合はtrue</param>
        /// <returns>判定した文字コード、判定できなかった場合はnull</returns>
        public static Encoding GetJpEncoding(byte[] bytes, bool readAll = false)
        {
            int len = bytes.Length;

            //BOM判定
            if (len >= 2 && bytes[0] == 0xfe && bytes[1] == 0xff)//UTF-16BE
            {
                return Encoding.BigEndianUnicode;
            }
            else if (len >= 2 && bytes[0] == 0xff && bytes[1] == 0xfe)//UTF-16LE
            {
                return Encoding.Unicode;
            }
            else if (len >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)//UTF-8
            {
                return new UTF8Encoding(true, true);
            }
            else if (len >= 3 && bytes[0] == 0x2b && bytes[1] == 0x2f && bytes[2] == 0x76)//UTF-7
            {
                return Encoding.UTF7;
            }
            else if (len >= 4 && bytes[0] == 0x00 && bytes[1] == 0x00 && bytes[2] == 0xfe && bytes[3] == 0xff)//UTF-32BE
            {
                return new UTF32Encoding(true, true);
            }
            else if (len >= 4 && bytes[0] == 0xff && bytes[1] == 0xfe && bytes[2] == 0x00 && bytes[3] == 0x00)//UTF-32LE
            {
                return new UTF32Encoding(false, true);
            }

            //文字コード判定と日本語の文章らしさをまとめて確認

            //Shift_JIS判定用
            bool sjis = true;         //すべてのバイトがShift_JISで使用するバイト範囲かどうか
            bool sjis_2ndbyte = false;//次回の判定がShift_JISの2バイト目の判定かどうか
            bool sjis_kana = false;   //かな判定用
            bool sjis_kanji = false;  //常用漢字判定用
            int counter_sjis = 0;     //Shift_JISらしさ

            //UTF-8判定用
            bool utf8 = true;            //すべてのバイトがUTF-8で使用するバイト範囲かどうか
            bool utf8_multibyte = false; //次回の判定がUTF-8の2バイト目以降の判定かどうか
            bool utf8_kana_kanji = false;//かな・常用漢字判定用
            int counter_utf8 = 0;        //UTF-8らしさ
            int counter_utf8_multibyte = 0;

            //EUC-JP判定用
            bool eucjp = true;            //すべてのバイトがEUC-JPで使用するバイト範囲かどうか
            bool eucjp_multibyte = false; //次回の判定がEUC-JPの2バイト目以降の判定かどうか
            bool eucjp_kana_kanji = false;//かな・常用漢字判定用
            int counter_eucjp = 0;        //EUC-JPらしさ
            int counter_eucjp_multibyte = 0;

            for (int i = 0; i < len; i++)
            {
                byte b = bytes[i];

                //Shift_JIS判定
                if (sjis)
                {
                    if (!sjis_2ndbyte)
                    {
                        if (b == 0x0D                   //CR
                            || b == 0x0A                //LF
                            || b == 0x09                //tab
                            || (0x20 <= b && b <= 0x7E))//ASCII文字
                        {
                            counter_sjis++;
                        }
                        else if ((0x81 <= b && b <= 0x9F) || (0xE0 <= b && b <= 0xFC))//Shift_JISの2バイト文字の1バイト目の場合
                        {
                            //2バイト目の判定を行う
                            sjis_2ndbyte = true;

                            if (0x82 <= b && b <= 0x83)//Shift_JISのかな
                            {
                                sjis_kana = true;
                            }
                            else if ((0x88 <= b && b <= 0x9F) || (0xE0 <= b && b <= 0xE3) || b == 0xE6 || b == 0xE7)//Shift_JISの常用漢字
                            {
                                sjis_kanji = true;
                            }
                        }
                        else if (0xA1 <= b && b <= 0xDF)//Shift_JISの1バイト文字の場合(半角カナ)
                        {
                            ;
                        }
                        else if (0x00 <= b && b <= 0x7F)//ASCIIコード
                        {
                            ;
                        }
                        else
                        {
                            //Shift_JISでない
                            counter_sjis = 0;
                            sjis = false;
                        }
                    }
                    else
                    {
                        if ((0x40 <= b && b <= 0x7E) || (0x80 <= b && b <= 0xFC))//Shift_JISの2バイト文字の2バイト目の場合
                        {
                            if (sjis_kana && 0x40 <= b && b <= 0xF1)//Shift_JISのかな
                            {
                                counter_sjis += 2;
                            }
                            else if (sjis_kanji && 0x40 <= b && b <= 0xFC && b != 0x7F)//Shift_JISの常用漢字
                            {
                                counter_sjis += 2;
                            }

                            sjis_2ndbyte = sjis_kana = sjis_kanji = false;
                        }
                        else
                        {
                            //Shift_JISでない
                            counter_sjis = 0;
                            sjis = false;
                        }
                    }
                }

                //UTF-8判定
                if (utf8)
                {
                    if (!utf8_multibyte)
                    {
                        if (b == 0x0D                   //CR
                            || b == 0x0A                //LF
                            || b == 0x09                //tab
                            || (0x20 <= b && b <= 0x7E))//ASCII文字
                        {
                            counter_utf8++;
                        }
                        else if (0xC2 <= b && b <= 0xDF)//2バイト文字の場合
                        {
                            utf8_multibyte = true;
                            counter_utf8_multibyte = 1;
                        }
                        else if (0xE0 <= b && b <= 0xEF)//3バイト文字の場合
                        {
                            utf8_multibyte = true;
                            counter_utf8_multibyte = 2;

                            if (b == 0xE3 || (0xE4 <= b && b <= 0xE9))
                            {
                                utf8_kana_kanji = true;//かな・常用漢字
                            }
                        }
                        else if (0xF0 <= b && b <= 0xF3)//4バイト文字の場合
                        {
                            utf8_multibyte = true;
                            counter_utf8_multibyte = 3;
                        }
                        else if (0x00 <= b && b <= 0x7F)//ASCIIコード
                        {
                            ;
                        }
                        else
                        {
                            //UTF-8でない
                            counter_utf8 = 0;
                            utf8 = false;
                        }
                    }
                    else
                    {
                        if (counter_utf8_multibyte > 0)
                        {
                            counter_utf8_multibyte--;

                            if (b < 0x80 || 0xBF < b)
                            {
                                //UTF-8でない
                                counter_utf8 = 0;
                                utf8 = false;
                            }
                        }

                        if (utf8 && counter_utf8_multibyte == 0)
                        {
                            if (utf8_kana_kanji)
                            {
                                counter_utf8 += 3;
                            }
                            utf8_multibyte = utf8_kana_kanji = false;
                        }
                    }
                }

                //EUC-JP判定
                if (eucjp)
                {
                    if (!eucjp_multibyte)
                    {
                        if (b == 0x0D                   //CR
                            || b == 0x0A                //LF
                            || b == 0x09                //tab
                            || (0x20 <= b && b <= 0x7E))//ASCII文字
                        {
                            counter_eucjp++;
                        }
                        else if (b == 0x8E || (0xA1 <= b && b <= 0xA8) || b == 0xAD || (0xB0 <= b && b <= 0xFE))//2バイト文字の場合
                        {
                            eucjp_multibyte = true;
                            counter_eucjp_multibyte = 1;

                            if (b == 0xA4 || b == 0xA5 || (0xB0 <= b && b <= 0xEE))
                            {
                                eucjp_kana_kanji = true;
                            }
                        }
                        else if (b == 0x8F)//3バイト文字の場合
                        {
                            eucjp_multibyte = true;
                            counter_eucjp_multibyte = 2;
                        }
                        else if (0x00 <= b && b <= 0x7F)//ASCIIコード
                        {
                            ;
                        }
                        else
                        {
                            //EUC-JPでない
                            counter_eucjp = 0;
                            eucjp = false;
                        }
                    }
                    else
                    {
                        if (counter_eucjp_multibyte > 0)
                        {
                            counter_eucjp_multibyte--;

                            if (b < 0xA1 || 0xFE < b)
                            {
                                //EUC-JPでない
                                counter_eucjp = 0;
                                eucjp = false;
                            }
                        }

                        if (eucjp && counter_eucjp_multibyte == 0)
                        {
                            if (eucjp_kana_kanji)
                            {
                                counter_eucjp += 2;
                            }
                            eucjp_multibyte = eucjp_kana_kanji = false;
                        }
                    }
                }

                //ISO-2022-JP
                if (b == 0x1B)
                {
                    if ((i + 2 < len && bytes[i + 1] == 0x24 && bytes[i + 2] == 0x40)                                                                           //1B-24-40
                        || (i + 2 < len && bytes[i + 1] == 0x24 && bytes[i + 2] == 0x42)                                                                        //1B-24-42
                        || (i + 2 < len && bytes[i + 1] == 0x28 && bytes[i + 2] == 0x4A)                                                                        //1B-28-4A
                        || (i + 2 < len && bytes[i + 1] == 0x28 && bytes[i + 2] == 0x49)                                                                        //1B-28-49
                        || (i + 2 < len && bytes[i + 1] == 0x28 && bytes[i + 2] == 0x42)                                                                        //1B-28-42
                        || (i + 3 < len && bytes[i + 1] == 0x24 && bytes[i + 2] == 0x48 && bytes[i + 3] == 0x44)                                                //1B-24-48-44
                        || (i + 3 < len && bytes[i + 1] == 0x24 && bytes[i + 2] == 0x48 && bytes[i + 3] == 0x4F)                                                //1B-24-48-4F
                        || (i + 3 < len && bytes[i + 1] == 0x24 && bytes[i + 2] == 0x48 && bytes[i + 3] == 0x51)                                                //1B-24-48-51
                        || (i + 3 < len && bytes[i + 1] == 0x24 && bytes[i + 2] == 0x48 && bytes[i + 3] == 0x50)                                                //1B-24-48-50
                        || (i + 5 < len && bytes[i + 1] == 0x26 && bytes[i + 2] == 0x40 && bytes[i + 3] == 0x1B && bytes[i + 4] == 0x24 && bytes[i + 5] == 0x42)//1B-26-40-1B-24-42
                    )
                    {
                        return Encoding.GetEncoding(50220);//iso-2022-jp
                    }
                }
            }

            // すべて読み取った場合で、最後が多バイト文字の途中で終わっている場合は判定NG
            if (readAll)
            {
                if (sjis && sjis_2ndbyte)
                {
                    sjis = false;
                }

                if (utf8 && utf8_multibyte)
                {
                    utf8 = false;
                }

                if (eucjp && eucjp_multibyte)
                {
                    eucjp = false;
                }
            }

            if (sjis || utf8 || eucjp)
            {
                //日本語らしさの最大値確認
                int max_value = counter_eucjp;
                if (counter_sjis > max_value)
                {
                    max_value = counter_sjis;
                }
                if (counter_utf8 > max_value)
                {
                    max_value = counter_utf8;
                }

                //文字コード判定
                if (max_value == counter_utf8)
                {
                    return new UTF8Encoding(false, true);//utf8
                }
                else if (max_value == counter_sjis)
                {
                    return Encoding.GetEncoding(932);//ShiftJIS
                }
                else
                {
                    return Encoding.GetEncoding(51932);//EUC-JP
                }
            }
            else
            {
                return null;
            }
        }
    }
}
