CREATE TABLE payments (
	id NVARCHAR PRIMARY KEY DEFAULT uuid_generate_v4(),
	correlation_id NVARCHAR NOT NULL UNIQUE,
	amount DECIMAL(10,2) NOT NULL,
	processor_type VARCHAR(10) NOT NULL CHECK (processor_type in ('default', 'fallback')),
	processed_at TIMESTAMPZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_processor_processed ON payments (processor_type, processed_at);