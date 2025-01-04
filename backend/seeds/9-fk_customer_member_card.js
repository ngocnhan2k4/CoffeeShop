/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  await knex.raw(`
    ALTER TABLE customer
    ADD CONSTRAINT fk_customer_member_card FOREIGN KEY (customer_type) REFERENCES member_card(card_name);
`);
};