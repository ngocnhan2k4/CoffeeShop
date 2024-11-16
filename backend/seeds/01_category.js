exports.seed = async function(knex) {
  await knex('category').del()   // Deletes ALL existing entries
  await knex('category').insert([
    {name: 'Trà sữa'},
    {name: 'Cà phê'},
    {name: 'Trà trái cây tươi'},
    {name: 'Sinh tố'},
    {name: 'Nước ép'}
  ]);
};