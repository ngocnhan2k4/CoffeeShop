exports.up = async function (knex) {
    await knex.raw(`
        CREATE TABLE category(  
            id int IDENTITY(0,1) primary key,
            name nvarchar(50) not null
        );

        CREATE TABLE drink(
            id int IDENTITY(0,1) primary key,
            name nvarchar(50) not null,
            size char(1) not null,
            price int not null,
            category_id int not null,
            description nvarchar(255),
            stock int not null,
            image nvarchar(100) null,
            foreign key(category_id) references category(id) ON DELETE CASCADE ON UPDATE CASCADE
        );

        CREATE TABLE invoice(
            id int IDENTITY(0,1) primary key,
            created_at date not null,
            total int not null,
            method nvarchar(50) not null,
            status nvarchar(50) not null,
            customer_name nvarchar(50) not null,
            has_delivery char not null
        );

        CREATE TABLE invoice_detail(
            invoice_id int not null,
            drink_id int not null ,
            quantity int not null,
            price int not null,
            note nvarchar(100),
            primary key(invoice_id, drink_id),
            foreign key(invoice_id) references invoice(id) on delete cascade ON UPDATE CASCADE,
            foreign key(drink_id) references drink(id) on delete cascade ON UPDATE CASCADE
        );

        CREATE TABLE delivery_invoice(
            invoice_id int not null,
            address nvarchar(255) not null,
            phone nvarchar(15) not null,
            shipping_fee int not null,
            primary key(invoice_id),
            foreign key(invoice_id) references invoice(id) on delete cascade ON UPDATE CASCADE
        );
    `);

};

exports.down = async function (knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS delivery_invoice;
        DROP TABLE IF EXISTS invoice_detail;
        DROP TABLE IF EXISTS invoice;
        DROP TABLE IF EXISTS drink;
        DROP TABLE IF EXISTS category;
    `);
};