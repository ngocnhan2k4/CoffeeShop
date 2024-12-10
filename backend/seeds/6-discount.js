exports.seed = async function (knex) {
    await knex('discount').del();

    await knex('discount').insert([
        {
            name: 'Summer Discount',
            discount_percent: 15,
            valid_until: '2025-08-31 23:59:59',
            category_id: 1,
            is_active: true
        },
        {
            name: 'Winter Discount',
            discount_percent: 20,
            valid_until: '2024-12-31 23:59:59',
            category_id: 2,
            is_active: true
        },
        {
            name: 'New Year Discount',
            discount_percent: 10,
            valid_until: '2025-01-31 23:59:59',
            category_id: 3,
            is_active: false
        }
    ]);
};
