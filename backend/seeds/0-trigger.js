/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function (knex) {
    // Drop triggers if they already exist
    await knex.raw(`
    IF OBJECT_ID('trg_decrease_stock', 'TR') IS NOT NULL
    DROP TRIGGER trg_decrease_stock;
  `);

    await knex.raw(`
    IF OBJECT_ID('trg_increase_stock_on_cancel', 'TR') IS NOT NULL
    DROP TRIGGER trg_increase_stock_on_cancel;
  `);

    await knex.raw(`
    CREATE TRIGGER trg_decrease_stock
    ON invoice_detail
    AFTER INSERT
    AS
    BEGIN
        UPDATE drink
        SET stock = stock - inserted.quantity
        FROM drink
        INNER JOIN inserted ON drink.id = inserted.drink_id
    END;
`);
    await knex.raw(`
    CREATE TRIGGER trg_increase_stock_on_cancel
    ON invoice
    AFTER UPDATE
    AS
    BEGIN
        IF UPDATE(status)
        BEGIN
            UPDATE drink
            SET stock = stock + invoice_detail.quantity
            FROM drink
            INNER JOIN invoice_detail ON drink.id = invoice_detail.drink_id
            INNER JOIN inserted ON invoice_detail.invoice_id = inserted.id
            WHERE inserted.status = 'Cancel';
        END
    END;
`);
};
