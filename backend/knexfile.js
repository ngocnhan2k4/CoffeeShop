const dotenv = require('dotenv');

const env = process.env.NODE_ENV || 'development';
dotenv.config({ path: `.env.${env}` });

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
  },
  test: {
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
