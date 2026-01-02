-- Direct SQL script to add missing columns
-- Execute this script manually against the database

-- Add BuyerId column
ALTER TABLE "Offers" ADD COLUMN IF NOT EXISTS "BuyerId" INTEGER;

-- Add CarrierId column  
ALTER TABLE "Offers" ADD COLUMN IF NOT EXISTS "CarrierId" INTEGER;

-- Add BuyerZipCode column
ALTER TABLE "Offers" ADD COLUMN IF NOT EXISTS "BuyerZipCode" VARCHAR(20);

-- Make VIN column nullable
ALTER TABLE "Offers" ALTER COLUMN "Vin" DROP NOT NULL;

-- Verify the columns were added
SELECT column_name, data_type, is_nullable 
FROM information_schema.columns 
WHERE table_name = 'Offers' 
  AND column_name IN ('BuyerId', 'CarrierId', 'BuyerZipCode', 'Vin')
ORDER BY column_name;