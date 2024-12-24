exports.up = async function (knex) {
    await knex.raw(`
        ALTER TABLE invoice
        ADD member_card_id INT NULL;
    `);
};

exports.down = async function (knex) {
    await knex.raw(`
        ALTER TABLE invoice
        DROP COLUMN member_card_id;
    `);
};