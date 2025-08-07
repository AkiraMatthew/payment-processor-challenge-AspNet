--DROP INDEX IF EXISTS idx_processor_processed;
--DROP TABLE IF EXISTS payments;

CREATE TABLE payments (
    Correlation_Id UUID UNIQUE,
    Amount DECIMAL(20, 2) NOT NULL,
    Processor_Type VARCHAR(10), --NOT NULL CHECK (processor_type IN ('default', 'fallback')),
    Processed_At TIMESTAMPTZ
);

CREATE INDEX idx_processor_processed ON payments (processor_type, processed_at);