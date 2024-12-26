/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('member_card').del()
  await knex('member_card').insert([
    {card_name: 'Thẻ thành viên', discount: 5},
    {card_name: 'Thẻ bạc', discount: 10},
    {card_name: 'Thẻ vàng', discount: 15},
  ]);
};
