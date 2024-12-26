exports.seed = async function (knex) {
  await knex('customer').del();
  await knex('customer').insert([
      {
          customer_name: 'Trần Văn A',
          total_money: 0,
          total_point: 0,
          customer_type: 'Thẻ thành viên'
      },
      {
          customer_name: 'Trần Văn B',
          total_money: 0,
          total_point: 0,
          customer_type: 'Thẻ thành viên'
      },
      {
          customer_name: 'Trần Văn C',
          total_money: 0,
          total_point: 0,
          customer_type: 'Thẻ thành viên'
      },
      {
          customer_name: 'Phạm Văn C',
          total_money: 0,
          total_point: 0,
          customer_type: 'Thẻ thành viên'
      },
      {
        customer_name: 'Trần Văn F',
        total_money: 0,
        total_point: 0,
        customer_type: 'Thẻ thành viên'
      },
      {
        customer_name: 'Trần Văn KK',
        total_money: 0,
        total_point: 0,
        customer_type: 'Thẻ thành viên'
      },
  ]);
};