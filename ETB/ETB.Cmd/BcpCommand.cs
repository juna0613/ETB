using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETB.Cmd
{
    public class BcpOutCommand : ICommand
    {
        const string SERVER_OPT = "-S";
        const string TRUSTED_CONN_OPT = "-T";
        const string PASSWORD_OPT = "-P";
        const string USER_OPT = "-U";
        const string DELIMITER_OPT = "-t";

        private readonly string _server, _db, _table, _outPath, _delimiter, _id, _password;
        private readonly bool _isTrustedConn;
        public BcpOutCommand(string server, string db, string table, string outputPath)
        {
            
        }
        public BcpOutCommand(string server, string db, string table, string outputPath, string delimiter)
        {

        }
        
        public BcpOutCommand(string server, string db, string table, string outputPath, string id, string password)
        {
            _server = server;
            _db = db;
            _table = table;
            _outPath = outputPath;
        }



        #region ICommand メンバ

        public CommandStatus DoCommand()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
