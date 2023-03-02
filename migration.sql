CREATE TABLE IF NOT EXISTS customers (
    id VARCHAR(50) PRIMARY KEY,
    available_amount NUMERIC,
    waiting_funds_amount NUMERIC,
    updated_at TIMESTAMP NOT NULL
);

CREATE TABLE IF NOT EXISTS transactions (
    id UUID PRIMARY KEY,
    amount NUMERIC NOT NULL,
    description VARCHAR(80),
    payment_method VARCHAR(15) NOT NULL,
    card_number VARCHAR(16),
    card_holder_name VARCHAR(40),
    card_cvv VARCHAR(3),
    card_expiration_date DATE,
    created_at TIMESTAMP,
    customer_id VARCHAR(50) NOT NULL REFERENCES customers(id)
);

CREATE TABLE IF NOT EXISTS payables (
    id UUID PRIMARY KEY,
    status VARCHAR(15) NOT NULL,
    payment_date DATE NOT NULL,
    amount NUMERIC NOT NULL,
    transaction_id UUID UNIQUE NOT NULL REFERENCES transactions(id),
    customer_id VARCHAR(50) NOT NULL REFERENCES customers(id)
);

CREATE INDEX id_idx ON customers (id);
-- EXPLAIN SELECT * FROM customers WHERE id = 'isaac' FETCH FIRST 1 ROW ONLY

CREATE INDEX created_at_idx ON transactions (created_at);
-- EXPLAIN SELECT * FROM transactions ORDER BY created_at DESC;

CREATE INDEX transaction_id_idx ON payables (transaction_id);
-- EXPLAIN SELECT * FROM payables WHERE transaction_id = 'e58a3735-304e-4cb0-bebb-5d0c62ee76fa';

INSERT INTO public."customers"("id", "available_amount", "waiting_funds_amount", "updated_at") VALUES('customer1', 0.0, 0.0, now() at time zone ('utc'));