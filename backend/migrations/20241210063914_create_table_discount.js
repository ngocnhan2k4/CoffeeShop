exports.up = async function (knex) {
    await knex.raw(`
        CREATE TABLE discount (
            id int IDENTITY(1,1) primary key,
            name nvarchar(50) not null,
            discount_percent float not null,
            valid_until datetime not null,
            category_id int not null,
            is_active bit not null default 0
        );
    `);
};

exports.down = async function (knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS discount;
    `);
};