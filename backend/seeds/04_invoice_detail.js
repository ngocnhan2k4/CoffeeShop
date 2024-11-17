exports.seed = async function(knex) {
  await knex('invoice_detail').del()   // Deletes ALL existing entries
  await knex('invoice_detail').insert([
    {invoice_id:0,drink_id:0,quantity:1,price:25000},
    {invoice_id:1,drink_id:1,quantity:1,price:27000},
    {invoice_id:2,drink_id:2,quantity:1,price:25000},
    {invoice_id:3,drink_id:3,quantity:1,price:25000},
    {invoice_id:4,drink_id:4,quantity:1,price:25000},
  ]);
};