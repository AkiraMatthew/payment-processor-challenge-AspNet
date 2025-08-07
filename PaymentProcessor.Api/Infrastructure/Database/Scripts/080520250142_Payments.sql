--DROP INDEX IF EXISTS idx_processor_processed;
--DROP TABLE IF EXISTS payments;

CREATE TABLE payments (
    correlation_id UUID UNIQUE,
    amount DECIMAL(20, 2) NOT NULL,
    processor_type VARCHAR(10) CHECK (processor_type IN ('default', 'fallback')),
    processed_at TIMESTAMPTZ DEFAULT NOW()
);

CREATE INDEX idx_processor_processed ON payments (processor_type, processed_at);