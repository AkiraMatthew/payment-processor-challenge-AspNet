--DROP INDEX IF EXISTS idx_processor_processed;
--DROP TABLE IF EXISTS payments;

CREATE TABLE payments (
    id VARCHAR(36) PRIMARY KEY,
    correlation_id VARCHAR(36) NOT NULL UNIQUE,
    amount DECIMAL(10,2) NOT NULL,
    processor_type VARCHAR(10) NOT NULL CHECK (processor_type IN ('default', 'fallback')),
    processed_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_processor_processed ON payments (processor_type, processed_at);