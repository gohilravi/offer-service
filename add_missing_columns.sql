-- Script to add missing columns to the Offers table
-- This script addresses the issue where BuyerId and CarrierId columns are missing

-- Add BuyerId column if it doesn't exist
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_name = 'Offers' AND column_name = 'BuyerId') THEN
        ALTER TABLE "Offers" ADD COLUMN "BuyerId" INTEGER;
    END IF;
END $$;

-- Add CarrierId column if it doesn't exist  
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_name = 'Offers' AND column_name = 'CarrierId') THEN
        ALTER TABLE "Offers" ADD COLUMN "CarrierId" INTEGER;
    END IF;
END $$;

-- Add BuyerZipCode column if it doesn't exist
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_name = 'Offers' AND column_name = 'BuyerZipCode') THEN
        ALTER TABLE "Offers" ADD COLUMN "BuyerZipCode" VARCHAR(20);
    END IF;
END $$;

-- Make VIN column nullable if it's currently NOT NULL
DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns 
               WHERE table_name = 'Offers' AND column_name = 'Vin' AND is_nullable = 'NO') THEN
        ALTER TABLE "Offers" ALTER COLUMN "Vin" DROP NOT NULL;
    END IF;
END $$;

-- Verify the changes
SELECT 
    column_name,
    data_type,
    is_nullable,
    character_maximum_length
FROM information_schema.columns 
WHERE table_name = 'Offers' 
    AND column_name IN ('BuyerId', 'CarrierId', 'BuyerZipCode', 'Vin')
ORDER BY column_name;