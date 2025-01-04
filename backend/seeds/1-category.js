exports.seed = async function (knex) {
  await knex('category').del()   // Deletes ALL existing entries
  await knex('category').insert([
    { id: 0, name: 'Trà sữa' },
    { id: 1, name: 'Cà phê' },
    { id: 2, name: 'Trà trái cây tươi' },
    { id: 3, name: 'Sinh tố' },
    { id: 4, name: 'Nước ép' }
  ]);
};