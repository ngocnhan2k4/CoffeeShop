exports.seed = async function(knex) {
  await knex('delivery_invoice').del()   // Deletes ALL existing entries
  await knex('delivery_invoice').insert([
    {invoice_id:1,address:'123 Nguyen Van Linh',phone:'0123456789',shipping_fee:10000}
  ]);
};