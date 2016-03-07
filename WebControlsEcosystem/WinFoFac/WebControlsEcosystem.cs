using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace WebControlsEcosystem
{
    /*
     * utility that may remain private from regular users
     * that allows for the dynamic creation of form controls
     * as needed fand also provides control property updates decoupled 
     * from source form(s) (so instantiating test controls is easier as well).
     * 
     * event handlers created as well for the controls
     * 
     */
    public class _Default : Page { }
    public class oneForm // = new set of ui controls to present
    {
        public string id { get; set; }
        oneForm() { }
    }
    public interface IFormFactory
    {
        /* interface to generate and return controls to caller;
         * notice that no form dependency exists - no form references are passed into derived classes
         * that way many different form consumers can request gui objects in a factory/form decoupled format
         */
        void handle_Events(Page page, object sender, EventArgs e);
        object returnControl(string id, string t, Type type, int x, int y);
        object o { get; set; }
    }
    public interface IControlProperties
    {
        /* enforce properties existence 
         */
        string id { get; set; }
        string controlType { get; set; }
        string controlText { get; set; }
        int xcord { get; set; }
        int ycord { get; set; }
    }
    
    [Serializable]
    public class manageControls : _Default, IFormFactory, IControlProperties, IPostBackEventHandler
    {
        /*
         * 
         * TODO - lock all control edits performed here (possible clash w.asp.net postback events, other events, etc)
         * 
         */
        public string id { get; set; }
        public string controlType { get; set; }
        public string controlText { get; set; }
        public int xcord { get; set; }
        public int ycord { get; set; }
        public object o { get; set; }
        public void RaisePostBackEvent() { }
        public delegate void objectEventDelegate();
        public class SerializeSession
        {
            /*
            * USE context.items for mobile compat? yes
            * 
            */

            public object[] args { get; set; }
            static void serialize(params object[] args)
            {
                HttpContext.Current.Session[Convert.ToInt16(args[0])] = args[1] + "," + args[2] + "," + args[3] + "," + args[4] + ",";
            }
            public SerializeSession() { }
            public void go()
            {
                serialize(args);
            }
        }
        public void RaisePostBackEvent(string s) { }
        public static class StoreControls // unused for now
        {
            public static List<Control> controlList { get; set; }
        }
        public void o_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("clicked");
            //objectEventDelegate connected = new objectEventDelegate(handle_Events);
            //connected = handle_Events;
            //connected(); // test fire event
            //connected = null; //  turn the delegated method "route" off  
        }
        public void handle_Events(Page page, object sender, EventArgs ea)
        {
            TextBox tb = new TextBox();
            page.Controls.Add(tb);
            HttpContext.Current.Response.Write("clicked again after 2nd event consumer");
        }
        public object createControl()
        {
            // o = returnControl(id,controlType, controlText, xcord, ycord);            
            return o;
        }
        public Type editProperties(string controlId)
        {
            string controlType = Convert.ToString(o.GetType());
            if (controlType == "TextBox")
            {
                return o.GetType();
            }
            return null;
        }
        /*public class txtControls : _Default
        {
            public TextBox txtBoxRef { get { return TextBox1; } set { //TextBox1 = value; } }
        }*/
        public object returnControl(string id, string t, Type type, int x, int y)
        {
            /*
             * record for next postback
             */
            //sessDict.serializeViewState(id,t,text,x,y);
            SerializeSession ss = new SerializeSession();
            /*ss.args[0] = id;
            ss.args[1] = t;
            ss.args[2] = "text in control";
            ss.args[3] = x;
            ss.args[4] = y;*/

            if (t == "Button")
            {
                Button o = new Button();
                o.ID = id;
                o.Attributes.Add("style", "Z-INDEX: 100; LEFT: " + y + "px; POSITION: absolute; TOP: " + x + "px");
                o.Text = "cntl text";
                //o.Click += new EventHandler();
                o.EnableViewState = true;
                ss.go();
                return o;
            }
            if (t == "TextBox")
            {
                if (id == "TextBox2")
                {
                    // txtControls tx = new txtControls();
                    //TextBox txtbox = tx.txtBoxRef;
                    //txtbox.Text = "it worked";
                    //txtbox.ID = "TextBox2";
                    //return txtbox;
                }
                else
                {
                    return null;
                }
            }
            if (t == "CheckBox")
            {
                CheckBox o = new CheckBox();
                o.Attributes.Add("style", "Z-INDEX: 100; LEFT: " + y + "px; POSITION: absolute; TOP: " + x + "px");
                o.Text = "";
                return o;
            }
            return null;
        }
    }
}
