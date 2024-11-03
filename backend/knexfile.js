require('dotenv').config();

module.exports = {
  development: {
    client: 'mssql',
    connection: {
      host: `${process.env.SQLSERVER_HOST}`,
      port: parseInt(process.env.SQLSERVER_PORT),
      user: `${process.env.SQLSERVER_USERNAME}`,
      password: `${process.env.SQLSERVER_PASSWORD}`,
      database: `${process.env.SQLSERVER_DATABASE}`,
    }
  }
};