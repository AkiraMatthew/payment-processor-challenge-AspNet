--DROP INDEX IF EXISTS idx_processor_processed;
--DROP TABLE IF EXISTS payments;

CREATE TABLE payments (
    correlation_id UUID PRIMARY KEY,
    Amount DECIMAL(20, 2) NOT NULL,
    requested_at TIMESTAMP NOT NULL CHECK (gateway IN ('default', 'fallback')),
    gateway TEXT NOT NULL
);

CREATE INDEX idx_payments_requested_at ON payments (gateway, requested_at);