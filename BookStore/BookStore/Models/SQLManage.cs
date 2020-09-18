using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;



namespace DateBaseTest
{
    public class BOOK
    {
        public int book_id;
        public string book_name;
        public int dept_id;
        public string category;
        public int score;
        public int cnt; //售卖次数
        public string state;  //出售状态
        public string describe;
        public string publishing_house;
        public int version;
        public BOOK(int _book_id, string _book_name, int _dept_id,
            string _category, string _publishing_house, int _version)
        {
            this.book_id = _book_id;
            this.book_name = _book_name;
            this.dept_id = _dept_id;
            this.category = _category;
            this.publishing_house = _publishing_house;
            this.version = _version;
        }
    }
    public class GOODS
    {
        public int good_id;
        public int book_id;
        public int price;
        public string good_description;
        public int saler_id;
        public int score;
        public string book_name;
        public GOODS(int _good_id, int _book_id, int _price, string _good_description)
        {
            this.good_id = _good_id;
            this.book_id = _book_id;
            this.price = _price;
            this.good_description = _good_description;
        }
    }
    class DEPT
    {
        public int dept_id;
        public string dept_name;
        public DEPT(int _dept_id, string _dept_name)
        {
            this.dept_id = _dept_id;
            this.dept_name = _dept_name;
        }
    }
    class PURCHASE
    {
        public int goods_id;
        public int buyer_id;
        public int saler_id;
        public int purchase_score;
        public string purchase_comment;
        public PURCHASE(int _goods_id, int _buyer_id, int _solder_id, int _purchase_score, string _purchase_comment)
        {
            this.goods_id = _goods_id;
            this.buyer_id = _buyer_id;
            this.saler_id = _solder_id;
            this.purchase_score = _purchase_score;
            this.purchase_comment = _purchase_comment;
        }
    }
    class BOOK_COMMENT
    {
        public int book_id;
        public int user_id;
        public int book_score;
        public string content;
        public string comment_time;
        public BOOK_COMMENT(int _book_id, int _user_id, int _book_score, string _content,string _time)
        {
            this.book_id = _book_id;
            this.user_id = _user_id;
            this.book_score = _book_score;
            this.content = _content;
            this.comment_time = comment_time;
        }
    }
    class PUBLISH
    {
        public int goods_id;
        public int user_id;
        public int book_id;
        public int publish_type;
        public PUBLISH(int _goods_id, int _user_id, int _book_id, int _publish_type)
        {
            this.goods_id = _goods_id;
            this.user_id = _user_id;
            this.book_id = _book_id;
            this.publish_type = _publish_type;
        }
    }
    class USER
    {
        public int user_id;
        public int honesty;
        public int age;
        public int dept_id;
        public string password;
        public string user_name;
        public string university;
        public string phone_number;
        public USER(int _user_id, int _honesty, int _age, int _dept_id,
            string _password, string _user_name, string _university, string _phone_number)
        {
            this.user_id = _user_id;
            this.honesty = _honesty;
            this.age = _age;
            this.dept_id = _dept_id;
            this.password = _password;
            this.user_name = _user_name;
            this.university = _university;
            this.phone_number = _phone_number;
        }
    }
    class Temp
    {
        public GOODS good;
        public int score;
        public string name;
    }
    class DateBaseCmds
    {
        static string connString = "server=120.25.145.41;database=二手图书管理系统;uid=root;pwd=pwd;charset=utf8";
        MySqlConnection conn = new MySqlConnection(connString);
        public void test_connect()
        {
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("ConnectFailed!!");
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public string QueryString(string SqlStr, string QueryItem)
        {
            using (MySqlCommand cmd = new MySqlCommand())//创建查询命令
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = SqlStr;
                MySqlDataReader reader = cmd.ExecuteReader();//创建一个执行命令的对象,但是还没有执行命令
                while (reader.Read())//按行执行查询，每次循环查询一行
                {
                    //此时reader会获取一行的内容，返回一个类似字典的结构，key为列名，value为值
                    //对于不同的数据库字段类型，我们需要用不同的方法获取
                    try
                    {
                        string item = reader.GetString(QueryItem);
                        Console.OutputEncoding = Encoding.UTF8;
                        return item;
                    }
                    catch
                    {
                        return null;
                    }
                }
                conn.Close();
            }
            return null;
        }
        public int QueryInt(string SqlStr, string QueryItem)
        {
            using (MySqlCommand cmd = new MySqlCommand())//创建查询命令
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = SqlStr;
                MySqlDataReader reader = cmd.ExecuteReader();//创建一个执行命令的对象,但是还没有执行命令
                while (reader.Read())//按行执行查询，每次循环查询一行
                {
                    //此时reader会获取一行的内容，返回一个类似字典的结构，key为列名，value为值
                    //对于不同的数据库字段类型，我们需要用不同的方法获取
                    try
                    {
                        int item = reader.GetInt32(QueryItem);
                        Console.OutputEncoding = Encoding.UTF8;
                        return item;
                    }
                    catch
                    {
                        return -1;
                    }
                }
                conn.Close();
            }
            return -1;
        }
        public List<BOOK> QueryBOOK(string SqlStr)
        {
            List<BOOK> books = new List<BOOK>();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = SqlStr;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int book_id = -1;
                    string book_name = null;
                    int dept_id = -1;
                    string category = null;
                    string publishing_house = null;
                    int version = -1;
                    try
                    {
                        book_id = reader.GetInt32("book_id");
                    }
                    catch
                    {
                        book_id = -1;
                    }
                    try
                    {
                        book_name = reader.GetString("book_name");
                    }
                    catch
                    {
                        book_name = null;
                    }
                    try
                    {
                        dept_id = reader.GetInt32("dept_id");
                    }
                    catch
                    {
                        dept_id = -1;
                    }
                    try
                    {
                        category = reader.GetString("category");
                    }
                    catch
                    {
                        category = null;
                    }
                    try
                    {
                        publishing_house = reader.GetString("publishing_house");
                    }
                    catch
                    {
                        publishing_house = null;
                    }
                    try
                    {
                        version = reader.GetInt32("version");
                    }
                    catch
                    {
                        version = -1;
                    }
                    BOOK book = new BOOK(book_id, book_name, dept_id, category,
                        publishing_house, version);
                    books.Add(book);
                }
                conn.Close();
            }
            return books;
        }
        //public List <GOODS> QueryGOODS(string SqlStr)
        public List<GOODS> QueryGOODS(string SqlStr)
        {
            List<GOODS> goods = new List<GOODS>();
            using (MySqlCommand cmd = new MySqlCommand())//创建查询命令
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = SqlStr;
                MySqlDataReader reader = cmd.ExecuteReader();//创建一个执行命令的对象,但是还没有执行命令
                while (reader.Read())//按行执行查询，每次循环查询一行
                {
                    //此时reader会获取一行的内容，返回一个类似字典的结构，key为列名，value为值
                    //对于不同的数据库字段类型，我们需要用不同的方法获取
                    int goods_id = -1;
                    int book_id = -1;
                    int price = -1;
                    string good_description = null;
                    try
                    {
                        goods_id = reader.GetInt32("goods_id");
                    }
                    catch
                    {
                        goods_id = -1;
                    }
                    try
                    {
                        book_id = reader.GetInt32("book_id");
                    }
                    catch
                    {
                        book_id = -1;
                    }
                    try
                    {
                        price = reader.GetInt32("price");
                    }
                    catch
                    {
                        price = -1;
                    }
                    try
                    {
                        good_description = reader.GetString("goods_description");
                    }
                    catch
                    {
                        good_description = null;
                    }
                    //try....score,describe
                    GOODS good = new GOODS(goods_id, book_id, price, good_description);
                    goods.Add(good);
                }
                conn.Close();
            }
            return goods;
        }
        public List<BOOK_COMMENT> QueryBOOKCOMMENT(string SqlStr)
        {
            List<BOOK_COMMENT> book_comments = new List<BOOK_COMMENT>();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = SqlStr;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int goods_id = -1;
                    int user_id = -1;
                    int book_score = -1;
                    string content = null;
                    string time = null;
                    try
                    {
                        goods_id = reader.GetInt32("goods_id");
                    }
                    catch
                    {
                        goods_id = -1;
                    }
                    try
                    {
                        user_id = reader.GetInt32("user_id");
                    }
                    catch
                    {
                        user_id = -1;
                    }
                    try
                    {
                        book_score = reader.GetInt32("book_score");
                    }
                    catch
                    {
                        book_score = -1;
                    }
                    try
                    {
                        content = reader.GetString("content");
                    }
                    catch
                    {
                        content = null;
                    }
                    try
                    {
                        time = reader.GetString("comment_time");
                    }
                    catch
                    {
                        time = null;
                    }
                    BOOK_COMMENT book_comment
                        = new BOOK_COMMENT(goods_id, user_id, book_score, content,time);
                    book_comments.Add(book_comment);
                }
                conn.Close();
            }
            return book_comments;
        }
        public List<PURCHASE> QueryPURCHASE(string SqlStr)
        {
            List<PURCHASE> purchases = new List<PURCHASE>();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = SqlStr;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int goods_id = -1;
                    int buyer_id = -1;
                    int saler_id = -1;
                    int purchase_score = -1;
                    string purchase_comment = null;
                    try
                    {
                        goods_id = reader.GetInt32("goods_id");
                    }
                    catch
                    {
                        goods_id = -1;
                    }
                    try
                    {
                        buyer_id = reader.GetInt32("buyer_id");
                    }
                    catch
                    {
                        buyer_id = -1;
                    }
                    try
                    {
                        saler_id = reader.GetInt32("saler_id");
                    }
                    catch
                    {
                        saler_id = -1;
                    }
                    try
                    {
                        purchase_score = reader.GetInt32("purchase_score");
                    }
                    catch
                    {
                        purchase_score = -1;
                    }
                    try
                    {
                        purchase_comment = reader.GetString("purchase_comment");
                    }
                    catch
                    {
                        purchase_comment = null;
                    }
                    PURCHASE purchase
                        = new PURCHASE(goods_id, buyer_id, saler_id, purchase_score, purchase_comment);
                    purchases.Add(purchase);
                }
                conn.Close();
            }
            return purchases;
        }

        public List<PUBLISH> QueryPUBLISH(string SqlStr)
        {
            List<PUBLISH> publishs = new List<PUBLISH>();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = SqlStr;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int goods_id = -1;
                    int user_id = -1;
                    int book_id = -1;
                    int publish_type = -2;
                    try
                    {
                        goods_id = reader.GetInt32("goods_id");
                    }
                    catch
                    {
                        goods_id = -1;
                    }
                    try
                    {
                        user_id = reader.GetInt32("user_id");
                    }
                    catch
                    {
                        user_id = -1;
                    }
                    try
                    {
                        book_id = reader.GetInt32("book_id");
                    }
                    catch
                    {
                        book_id = -1;
                    }
                    try
                    {
                        publish_type = reader.GetInt32("publish_type");
                    }
                    catch
                    {
                        publish_type = -1;
                    }
                    PUBLISH publish
                        = new PUBLISH(goods_id, user_id, book_id, publish_type);
                    publishs.Add(publish);
                }
                conn.Close();
            }
            return publishs;
        }
        public List<USER> QueryUSER(string SqlStr)
        {
            List<USER> users = new List<USER>();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = SqlStr;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int user_id = -1;
                    int honesty = -1;
                    int age = -1;
                    int dept_id = -1;
                    string password = null;
                    string user_name = null;
                    string university = null;
                    string phone_number = null;
                    try
                    {
                        user_id = reader.GetInt32("user_id");
                    }
                    catch
                    {
                        user_id = -1;
                    }
                    try
                    {
                        honesty = reader.GetInt32("honesty");
                    }
                    catch
                    {
                        honesty = -1;
                    }
                    try
                    {
                        age = reader.GetInt32("age");
                    }
                    catch
                    {
                        age = -1;
                    }
                    try
                    {
                        dept_id = reader.GetInt32("dept_id");
                    }
                    catch
                    {
                        dept_id = -1;
                    }
                    try
                    {
                        password = reader.GetString("password");
                    }
                    catch
                    {
                        password = null;
                    }
                    try
                    {
                        user_name = reader.GetString("user_name");
                    }
                    catch
                    {
                        user_name = null;
                    }
                    try
                    {
                        university = reader.GetString("university");
                    }
                    catch
                    {
                        university = null;
                    }
                    try
                    {
                        phone_number = reader.GetString("phone_number");
                    }
                    catch
                    {
                        phone_number = null;
                    }
                    USER user
                        = new USER(user_id, honesty, age, dept_id,
                            password, user_name, university, phone_number);
                    users.Add(user);
                }
                conn.Close();
            }
            return users;
        }
        public int QueryDeptId(string SqlStr)
        {
            int dept_id = -1;
            using (MySqlCommand cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = SqlStr;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        dept_id = reader.GetInt32("dept_id");
                    }
                    catch
                    {
                        dept_id = -1;
                    }
                }
                conn.Close();
            }
            return dept_id;
        }
        public void UpdateInsertDelete(string SqlStr)
        {
            using (MySqlCommand cmd = new MySqlCommand())//创建查询命令
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = SqlStr;
                cmd.ExecuteNonQuery();//用来执行sql语句，可用于增删改
                conn.Close();
            }
        }

        public List<DEPT> DeptHelp(string SqlStr)
        {
            List<DEPT> depts = new List<DEPT>();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = SqlStr;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int dept_id = -1;
                    string dept_name = null;
                    try
                    {
                        dept_id = reader.GetInt32("dept_id");
                    }
                    catch
                    {
                        dept_id = -1;
                    }
                    try
                    {
                        dept_name = reader.GetString("dept_name");
                    }
                    catch
                    {
                        dept_name = null;
                    }
                    DEPT dept = new DEPT(dept_id, dept_name);
                    depts.Add(dept);
                }
                conn.Close();
            }
            return depts;
        }
    }
}