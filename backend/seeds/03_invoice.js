exports.seed = async function (knex) {
  await knex('invoice').del()   // Deletes ALL existing entries
  await knex('invoice').insert([
    { created_at: '2024-10-16', total: 18000, method: 'Credit Card', status: 'Paid', customer_name: 'Nguyen Van A', has_delivery: 'N' },
    { created_at: '2024-10-15', total: 27000, method: 'Credit Card', status: 'Paid', customer_name: 'Nguyen Van B', has_delivery: 'Y' },
    { created_at: '2024-10-16', total: 15000, method: 'Credit Card', status: 'Paid', customer_name: 'Nguyen Van C', has_delivery: 'N' },
    { created_at: '2023-10-16', total: 15000, method: 'Credit Card', status: 'Paid', customer_name: 'Nguyen Van D', has_delivery: 'N' },
    { created_at: '2023-9-16', total: 27000, method: 'Credit Card', status: 'Paid', customer_name: 'Nguyen Van E', has_delivery: 'N' }
  ]);
};