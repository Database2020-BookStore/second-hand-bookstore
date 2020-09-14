using System;
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
            return datebase_connect.QueryString("select * from user where user_id=" + user_id.ToString(), "password") != null;
        }
        public void add_new_account(int user_id, string password, string user_name, int age, string university, string phone_number, int dept_id)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            //Console.WriteLine("insert into user values(1," + user_id.ToString() + ",'" + password + "','" + user_name + "'," + age.ToString() + ",'" + university + "','" + phone_number + "'," + dept_id.ToString() + ")");
            datebase_connect.UpdateInsertDelete("insert into user values(1," + user_id.ToString() + ",'" + password + "','" + user_name + "'," + age.ToString() + ",'" + university + "','" + phone_number + "'," + dept_id.ToString() + ")");
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
        public List<GOODS> get_all_goods()
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            List<GOODS> goods_list = new List<GOODS>();
            goods_list = datebase_connect.QueryGOODS("select * from goods");
            return goods_list;
        }
        public List<GOODS> get_all_goods_from_book(int book_id)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            List<GOODS> goods_list = new List<GOODS>();
            goods_list = datebase_connect.QueryGOODS("select * from goods where book_id=" + book_id.ToString());
            return goods_list;
        }
        public BOOK get_only_book(string book_name, string publishing_house, int version)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            BOOK book = null;
            if (publishing_house == null || version == -1)
            {
                return null;
            }
            List<BOOK> books = datebase_connect.QueryBOOK("select * from book where book_name='" + book_name
                + "' and publishing_house='" + publishing_house
                + "' and version=" + version.ToString());
            if (books.Count == 0) return null;
            else book = books[0];
            return book;
        }
        public List<GOODS> get_solding_book()
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            List<GOODS> solding_book_list = new List<GOODS>();
            solding_book_list = datebase_connect.QueryGOODS("select * from goods where price>0");
            return solding_book_list;
        }
        public void add_book(string book_name, int dept_id, string category,
            string publishing_house = null, int version = -1)
        {
            DateBaseCmds datebase_connect = new DateBaseCmds();
            datebase_connect.test_connect();
            BOOK book = get_only_book(book_name, publishing_house, version);
            if (book == null)
            {
                Console.WriteLine("book existed");
                return;
            }
            int book_id_generator = assign_book_id();
            string insertion_sql = "insert into book values(" + book_id_generator.ToString() + ",'" + book_name + "'," + dept_id.ToString() + ",'" + category + "')";
            Console.Write(insertion_sql);
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
        public void publish_goods(int book_id, int price,
            string good_description)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<BOOK> book_list = database_connect.QueryBOOK("select * from book where book_id=" + book_id.ToString());
            if (book_list.Count == 0)
            {
                Console.WriteLine("Book not existed");
                return;
            }
            database_connect.UpdateInsertDelete(
                "insert into goods values(" + generate_good_id().ToString()
                + "," + book_id.ToString() + "," + price.ToString()
                + ",'" + good_description + "')"
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
                Console.WriteLine("good not exist");
                return null;
            }
            BOOK book = get_book(good.book_id);
            if (book == null)
            {
                Console.WriteLine("book not exist");
                return null;
            }
            return book.book_name;
        }
        public void buy_book(int goods_id, int user_id = -1)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            database_connect.UpdateInsertDelete("update goods " +
                "set price=-1 " +
                "where goods_id=" + goods_id.ToString());
        }
        public void borrow_book(int goods_id, int user_id = -1)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            database_connect.UpdateInsertDelete("update goods " +
                "set price=0 " +
                "where goods_id=" + goods_id.ToString());
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
                Console.WriteLine("book not exist");
                return;
            }
            database_connect.UpdateInsertDelete(
                "insert into book_comment values(" + book_id.ToString()
                + "," + user_id.ToString()
                + ",'" + comment
                + "'," + score.ToString()
                + ",'" + DateTime.Now.ToString()
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
        public List<USER> get_all_users()
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<USER> user_list =
                database_connect.QueryUSER(
                    "select * from user");
            return user_list;
        }
        public USER get_user_by_user_id(int user_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<USER> user_list =
                database_connect.QueryUSER(
                    "select * from user where user_id=" + user_id.ToString());
            USER user = user_list[0];
            return user;
        }
        public void log_book(BOOK book)
        {
            Console.WriteLine("book id:" + book.book_id);
            Console.WriteLine("book name:" + book.book_name);
            Console.WriteLine("category:" + book.category);
            Console.WriteLine("dept id:" + book.dept_id);
            Console.WriteLine("publishing house:" + book.publishing_house);
            Console.WriteLine("version:" + book.version);
        }
        public void log_user(USER user)
        {
            Console.WriteLine("user id:" + user.user_id);
            Console.WriteLine("user name:" + user.user_name);
            Console.WriteLine("age:" + user.age);
            Console.WriteLine("honesty:" + user.honesty);
            Console.WriteLine("phone number:" + user.phone_number);
            Console.WriteLine("university:" + user.university);
            Console.WriteLine("dept id:" + user.dept_id);
        }
        public void log_dept()
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<DEPT> dept_list =
                database_connect.DeptHelp(
                    "select * from department");
            foreach (DEPT dept in dept_list)
            {
                Console.WriteLine(dept.dept_id.ToString() + " " + dept.dept_name);
            }
        }
        public void log_goods(GOODS goods)
        {
            Console.WriteLine("goods id:" + goods.good_id);
            Console.WriteLine("goods-book id:" + goods.book_id);
            Console.WriteLine("price:" + goods.price);
            Console.WriteLine("description:'" + goods.good_description + "'");
        }
        public List<USER> buyer_id_list(int book_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<USER> users = new List<USER>();
            users = database_connect.QueryUSER("select * " +
                "from user " +
                "where user.user_id in " +
                "(select buyer_id" +
                " from purchase " +
                "where purchase.goods_id in (" +
                "select goods_id " +
                "from goods " +
                "where goods.book_id = " + book_id.ToString() + ")); ");
            return users;
        }
        public string get_book_img_url(int book_id)
        {
            string url = null;
            return url;
        }
        public int get_dept_id_by_name(string dept_name)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            int dept_id = -1;
            string sql_sentence = "select * from department where dept_name='" + dept_name + "'";
            dept_id = database_connect.QueryDeptId("select * from department where dept_name='" + dept_name + "'"); ;
            Console.WriteLine(sql_sentence);
            return dept_id;
        }
        public List<GOODS> get_books_borrow_id(int borrower_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<GOODS> goods = new List<GOODS>();
            goods = database_connect.QueryGOODS("select * " +
                "from goods " +
                "where goods.goods_id in " +
                "(select goods_id" +
                " from borrow " +
                "where borrower_id = " + borrower_id.ToString() + "); ");
            return goods;
        }
        public List<BOOK_COMMENT> get_comment_by_book_id(int book_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<BOOK_COMMENT> book_comment = new List<BOOK_COMMENT>();
            book_comment = database_connect.QueryBOOKCOMMENT("select * " +
                "from book_comment " +
                "where book_id = " + book_id.ToString() + "; ");
            return book_comment;
        }
        public List<PUBLISH> get_publish_by_user_id(int user_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<PUBLISH> publish = new List<PUBLISH>();
            publish = database_connect.QueryPUBLISH("select * " +
                "from publish " +
                "where user_id = " + user_id.ToString() + "; ");
            return publish;
        }
        public PUBLISH get_publish_by_good_id(int good_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<PUBLISH> publish_list =
                database_connect.QueryPUBLISH(
                    "select * from publish where goods_id=" + good_id.ToString());
            if (publish_list.Count == 0)
            {
                return null;
            }
            PUBLISH publish = publish_list[0];
            return publish;
        }
        public int generate_user_id()
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<USER> user_list = database_connect.QueryUSER("select * from user");
            int key = 0;
            if (user_list.Count == 0)
            {
                return 1;
            }
            foreach (USER user in user_list)
            {
                if (user.user_id > key)
                {
                    key = user.user_id;
                }
            }
            key = key + 1;
            return key;
        }
        public List<GOODS> get_goods_by_book_id(int book_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<GOODS> goods_list = database_connect.QueryGOODS(
                "select * from goods where book_id =" + book_id.ToString()
            );
            return goods_list;
        }
        public List<GOODS> get_my_bought_goods(int user_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<GOODS> goods_list =
                database_connect.QueryGOODS(
                    "select goods_id,book_id,price,goods_description from purchase natural join goods where buyer_id = " + user_id.ToString());
            return goods_list;
        }
        int max(int a, int b)
        {
            return a > b ? a : b;
        }
        int cmpname(string a, string b)
        {
            if (a.Length < b.Length)
            {
                string c = b;
                b = a;
                a = c;
            }
            int n = a.Length, m = b.Length;
            System.Diagnostics.Debug.Assert(n < 20);//book_name should not so long,else run error
            int[,] dp = new int[25, 25];
            int[,] dc = new int[25, 25];
            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= m; j++)
                {
                    dp[i, j] = 0;
                }
            }
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    if (a[i - 1] == b[j - 1])
                    {
                        dp[i, j] = dp[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        dp[i, j] = max(dp[i - 1, j], dp[i, j - 1]);
                    }

                }
            }

            return dp[n, m];
        }
        public List<BOOK> SearchBooksByName(string name, int a)
        {
            //a is a varible that limit search range, when a=0 is return all the book,
            //and while a=name.length,it return book only prefect fit the name
            //rmal half of name.length is good

            DBops dbop = new DBops();
            List<BOOK> all = dbop.get_all_book();
            List<BOOK> ans = new List<BOOK>();
            foreach (BOOK one in all)
            {
                Console.WriteLine(one.book_name + cmpname(name, one.book_name).ToString());

                if (cmpname(name, one.book_name) >= a)
                {
                    ans.Add(one);
                }
            }
            return ans;
        }

        public List<GOODS> get_my_selled_goods(int user_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<GOODS> goods_list =
                database_connect.QueryGOODS(
                    "select goods_id,book_id,price,goods_description from purchase natural join goods where saler_id = " + user_id.ToString());
            return goods_list;
        }

        public List<GOODS> get_my_borrowed_goods(int user_id)
        {
            DateBaseCmds database_connect = new DateBaseCmds();
            database_connect.test_connect();
            List<GOODS> goods_list =
                database_connect.QueryGOODS(
                    "select goods_id,book_id,price,goods_description from borrow natural join goods where  borrower_id = " + user_id.ToString());
            return goods_list;
        }
    }
}