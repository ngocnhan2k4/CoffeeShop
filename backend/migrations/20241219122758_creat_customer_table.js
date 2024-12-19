exports.up = async function (knex) {
    await knex.raw(`
        CREATE TABLE customer (
            id int IDENTITY(1,1) primary key,
            customer_name nvarchar(255) not null,
            total_money decimal(10,2) not null,
            total_point FLOAT not null,
            customer_type nvarchar(255) not null
        );
    `);
};

exports.down = async function (knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS customer;
    `);
};