exports.up = async function (knex) {
    await knex.raw(`
        CREATE TRIGGER trg_update_total_point_and_customer_type
        ON customer
        AFTER UPDATE
        AS
        BEGIN
            UPDATE c
            SET c.total_point = i.total_money / 10000
            FROM customer c
            INNER JOIN inserted i
            ON c.id = i.id;

            UPDATE c
            SET c.customer_type = 
                CASE
                    WHEN c.total_point >= 100 THEN N'Thẻ vàng'
                    WHEN c.total_point >= 50 THEN N'Thẻ bạc'
                    ELSE N'Thẻ thành viên'
                END
            FROM customer c
            INNER JOIN inserted i
            ON c.id = i.id;
        END;

    `);
};

exports.down = async function (knex) {
    await knex.raw(`
        DROP TRIGGER IF EXISTS trg_update_total_point_and_customer_type;
    `);
};