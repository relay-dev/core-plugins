sqlcmd -S localhost\SQLEXPRESS -i sql\core-ddl-100-pre-process.sql

sqlcmd -S localhost\SQLEXPRESS -i sql\core-ddl-200-process.sql

sqlcmd -S localhost\SQLEXPRESS -i sql\core-ddl-300-post-process.sql

sqlcmd -S localhost\SQLEXPRESS -i sql\core-dml-100-pre-process.sql

sqlcmd -S localhost\SQLEXPRESS -i sql\core-dml-200-process.sql

sqlcmd -S localhost\SQLEXPRESS -i sql\core-dml-300-post-process.sql