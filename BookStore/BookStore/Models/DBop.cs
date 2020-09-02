using DateBaseTest;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DBop
{
    class DBops
    {
        int book_id_lim = 10000;
        public string get_password(int user_id)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            return datebase_connect.QueryString("select * from user where user_id=" + user_id.ToString(), "password");
        }
        public int get_user_credit(int user_id)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            return datebase_connect.QueryInt("select * from user where user_id=" + user_id.ToString(), "honesty");
        }
        public bool user_exist(int user_id)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            return datebase_connect.QueryString("select * from user where user_id=" + user_id.ToString(), "passwrod") != null;
        }
        public void add_new_account(int user_id, string password, string user_name, string phone_number, int dept_id)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            //Console.WriteLine("insert into user values(1," + user_id.ToString() + ",'" + password + "','" + user_name + "',1,'Tongji','" + phone_number + "'," + dept_id.ToString() + ")");
            datebase_connect.UpdateInsertDelete("insert into user values(1," + user_id.ToString() + ",'" + password + "','" + user_name + "',1,'Tongji','" + phone_number + "'," + dept_id.ToString() + ")");
        }
        public void change_name(int user_id, string name)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            //Console.WriteLine("update user set user_name='" + name + "' where user_id=" + user_id.ToString());
            datebase_connect.UpdateInsertDelete("update user set user_name='" + name + "' where user_id=" + user_id.ToString());
        }
        public void change_phone_number(int user_id, string phone_number)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            datebase_connect.UpdateInsertDelete("update user set phone_number='" + phone_number + "' where user_id=" + user_id.ToString());
        }
        public void change_password(int user_id, string password)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            datebase_connect.UpdateInsertDelete("update user set password='" + password + "' where user_id=" + user_id.ToString());
        }
        public void change_dept(int user_id, int dept_id)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            datebase_connect.UpdateInsertDelete("update user set dept_id=" + dept_id.ToString() + " where user_id=" + user_id.ToString());
        }
        public void change_credit(int user_id, int credit)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            datebase_connect.UpdateInsertDelete("update user set honesty=" + credit.ToString() + " where user_id=" + user_id.ToString());
        }
        public List<BOOK> get_all_book()
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            List<BOOK> book_list = new List<BOOK>();
            book_list = datebase_connect.QueryBOOK("select * from book");
            return book_list;
        }
        public List<GOODS> get_solding_book()
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            List<GOODS> solding_book_list = new List<GOODS>();
            solding_book_list = datebase_connect.QueryGOODS("select * from goods where price>=0");
            return solding_book_list;
        }
        public void add_book(string book_name, int dept_id, string category)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            int book_id_generator = assign_book_id();
            string insertion_sql = "insert into book values(" + book_id_generator.ToString() + ",'" + book_name + "'," + dept_id.ToString() + ",'" + category + "')";
            //Console.Write(insertion_sql);
            datebase_connect.UpdateInsertDelete(insertion_sql);
        }
        private int assign_book_id()
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<BOOK> book_list = database_connect.QueryBOOK("select * from book");
            int key = 0;
            if (book_list.Count == 0)
            {
                return 1;
            }
            foreach (BOOK book in book_list)
            {
                if (book.book_id > key)
                {
                    key = book.book_id;
                }
            }
            key = key + 1;
            return key;
        }
        public void publish_goods(int book_id, string book_name, int price,
            string good_description = null, int dept_id = -1, string category = null)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<BOOK> book_list = database_connect.QueryBOOK("select * from book where book_id=" + book_id.ToString());
            if (book_list.Count == 0)
            {
                if (dept_id != -1)
                {
                    database_connect.UpdateInsertDelete(
                        "insert into book values(" + book_id.ToString() + "," + book_name + "," + dept_id.ToString() + "," + category + ")"
                    );
                }
                else
                {
                    //Console.WriteLine("Initialize the book first");
                    return;
                }
            }
            database_connect.UpdateInsertDelete(
                "insert into goods values(" + generate_good_id().ToString() + "," + good_description + ")"
                );
        }
        private int generate_good_id()
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<GOODS> goods_list = database_connect.QueryGOODS("select * from goods");
            int key = 0;
            if (goods_list.Count == 0)
            {
                return 1;
            }
            foreach (GOODS goods in goods_list)
            {
                if (goods.good_id > key)
                {
                    key = goods.good_id;
                }
            }
            key = key + 1;
            return key;
        }
        public void erase_goods(int goods_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            database_connect.UpdateInsertDelete(
                "delete from goods where good_id=" + goods_id.ToString()
            );
        }
        public GOODS get_goods(int goods_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<GOODS> goods_list = database_connect.QueryGOODS(
                "select * from goods where goods_id =" + goods_id.ToString()
            );
            if (goods_list.Count == 0)
            {
                return null;
            }
            return goods_list[0];
        }
        public BOOK get_book(int book_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<BOOK> book_list = database_connect.QueryBOOK(
                "select * from book where book_id =" + book_id.ToString()
            );
            if (book_list.Count == 0)
            {
                return null;
            }
            return book_list[0];
        }
        public string get_book_name_from_good(int good_id)
        {
            GOODS good = get_goods(good_id);
            if (good == null)
            {
                //Console.WriteLine("good not exist");
                return null;
            }
            BOOK book = get_book(good.book_id);
            if (book == null)
            {
                //Console.WriteLine("book not exist");
                return null;
            }
            return book.book_name;
        }
        public void buy_book(int goods_id, int user_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();

        }
        public int get_average_score(int book_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<BOOK_COMMENT> book_comment_list = database_connect.QueryBOOKCOMMENT(
                "select * from book_comment where book_id =" + book_id.ToString()
            );
            if (book_comment_list.Count == 0)
            {
                return -1;
            }
            int score = 0;
            foreach (BOOK_COMMENT book_comment in book_comment_list)
            {
                score += book_comment.book_score;
            }
            return score / book_comment_list.Count;
        }
        public void give_comment_for_book(int book_id, string comment, int score, int user_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            if (get_book(book_id) == null)
            {
                //Console.WriteLine("book not exist");
                return;
            }
            database_connect.UpdateInsertDelete(
                "insert into book_comment values(" + book_id.ToString()
                + "," + user_id.ToString()
                + ",'" + comment
                + "'," + score.ToString()
                + ",'" + System.DateTime.Now.ToString()
                + "')"
            );
        }
        public List<GOODS> get_my_goods_information(int user_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<GOODS> goods_list =
                database_connect.QueryGOODS(
                    "select goods_id,book_id,price,goods_description from purchase natural join goods where saler_id = " + user_id.ToString());
            return goods_list;
        }
        public List<PURCHASE> get_my_trade_information(int user_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<PURCHASE> trade_list =
                database_connect.QueryPURCHASE(
                    "select * from purchase where saler_id=" + user_id.ToString());
            return trade_list;
        }

    }
}