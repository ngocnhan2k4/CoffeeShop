exports.up = async function (knex) {
    await knex.raw(`
        CREATE TABLE member_card (
            card_name nvarchar(255) primary key,
            discount int not null,
        );

        
    `);
};

exports.down = async function (knex) {
    await knex.raw(` 
        DROP TABLE IF EXISTS member_card;
    `);
};