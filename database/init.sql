-- Offer Service Database Schema
-- PostgreSQL Database Creation Script

-- Create database (run this separately with appropriate privileges)
-- CREATE DATABASE "OfferServiceDb";

-- Connect to the OfferServiceDb database before running the following

-- Create Sellers table
CREATE TABLE "Sellers" (
    "SellerId" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "Email" VARCHAR(255) NOT NULL UNIQUE,
    "PasswordHash" VARCHAR(255) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "LastModifiedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Create Offers table
CREATE TABLE "Offers" (
    "OfferId" BIGSERIAL PRIMARY KEY,
    "SellerId" INTEGER NOT NULL,
    "SellerNetworkId" VARCHAR(100) NOT NULL,
    "SellerName" VARCHAR(100) NOT NULL,
    
    -- Vehicle Identification Number
    "Vin" VARCHAR(17),
    
    -- Vehicle Information
    "VehicleYear" VARCHAR(50) NOT NULL,
    "VehicleMake" VARCHAR(100) NOT NULL,
    "VehicleModel" VARCHAR(100) NOT NULL,
    "VehicleTrim" VARCHAR(100),
    "VehicleBodyType" VARCHAR(100),
    "VehicleCabType" VARCHAR(100),
    "VehicleDoorCount" INTEGER DEFAULT 0,
    "VehicleFuelType" VARCHAR(100),
    "VehicleBodyStyle" VARCHAR(100),
    "VehicleUsage" VARCHAR(100),
    
    -- Location
    "VehicleZipCode" VARCHAR(20) NOT NULL,
    "BuyerZipCode" VARCHAR(20),
    
    -- Ownership
    "OwnershipType" VARCHAR(100),
    "OwnershipTitleType" VARCHAR(100),
    
    -- Condition
    "Mileage" INTEGER DEFAULT 0,
    "IsMileageUnverifiable" BOOLEAN DEFAULT FALSE,
    "DrivetrainCondition" VARCHAR(100),
    "KeyOrFobAvailable" VARCHAR(100),
    "WorkingBatteryInstalled" VARCHAR(100),
    "AllTiresInflated" VARCHAR(100),
    "WheelsRemoved" VARCHAR(100),
    "WheelsRemovedDriverFront" BOOLEAN DEFAULT FALSE,
    "WheelsRemovedDriverRear" BOOLEAN DEFAULT FALSE,
    "WheelsRemovedPassengerFront" BOOLEAN DEFAULT FALSE,
    "WheelsRemovedPassengerRear" BOOLEAN DEFAULT FALSE,
    "BodyPanelsIntact" VARCHAR(100),
    "BodyDamageFree" VARCHAR(100),
    "MirrorsLightsGlassIntact" VARCHAR(100),
    "InteriorIntact" VARCHAR(100),
    "FloodFireDamageFree" VARCHAR(100),
    "EngineTransmissionCondition" VARCHAR(100),
    "AirbagsDeployed" VARCHAR(100),
    
    -- Offer Meta
    "Status" VARCHAR(50) NOT NULL DEFAULT 'offered',
    "PurchaseId" UUID,
    "TransportId" UUID,
    "BuyerId" INTEGER,
    "CarrierId" INTEGER,
    "NoSQLIndexId" UUID NOT NULL DEFAULT gen_random_uuid(),
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "LastModifiedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    -- Foreign Key Constraint
    CONSTRAINT "FK_Offers_Sellers_SellerId" FOREIGN KEY ("SellerId") REFERENCES "Sellers"("SellerId") ON DELETE RESTRICT
);

-- Create indexes for better query performance
CREATE INDEX "IX_Offers_Status" ON "Offers"("Status");
CREATE INDEX "IX_Offers_CreatedAt" ON "Offers"("CreatedAt");
CREATE INDEX "IX_Offers_SellerId_Status" ON "Offers"("SellerId", "Status");
CREATE INDEX "IX_Sellers_Email" ON "Sellers"("Email");

-- Add check constraints for status validation
ALTER TABLE "Offers" ADD CONSTRAINT "CK_Offers_Status" 
    CHECK ("Status" IN ('offered', 'assigned', 'canceled'));

-- Insert seed data for Sellers (with BCrypt hashed passwords for 'password123')
INSERT INTO "Sellers" ("Name", "Email", "PasswordHash", "CreatedAt", "LastModifiedAt") VALUES
('John Doe', 'john.doe@example.com', '$2a$11$XABw.U9VE2xMKS9JDZXgK.xRQ0VYG1kGRYz4YW7uELC/qOxs6B2Gy', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('Jane Smith', 'jane.smith@example.com', '$2a$11$XABw.U9VE2xMKS9JDZXgK.xRQ0VYG1kGRYz4YW7uELC/qOxs6B2Gy', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('Bob Johnson', 'bob.johnson@example.com', '$2a$11$XABw.U9VE2xMKS9JDZXgK.xRQ0VYG1kGRYz4YW7uELC/qOxs6B2Gy', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('Alice Williams', 'alice.williams@example.com', '$2a$11$XABw.U9VE2xMKS9JDZXgK.xRQ0VYG1kGRYz4YW7uELC/qOxs6B2Gy', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('Charlie Brown', 'charlie.brown@example.com', '$2a$11$XABw.U9VE2xMKS9JDZXgK.xRQ0VYG1kGRYz4YW7uELC/qOxs6B2Gy', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('Diana Davis', 'diana.davis@example.com', '$2a$11$XABw.U9VE2xMKS9JDZXgK.xRQ0VYG1kGRYz4YW7uELC/qOxs6B2Gy', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('Edward Miller', 'edward.miller@example.com', '$2a$11$XABw.U9VE2xMKS9JDZXgK.xRQ0VYG1kGRYz4YW7uELC/qOxs6B2Gy', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('Fiona Wilson', 'fiona.wilson@example.com', '$2a$11$XABw.U9VE2xMKS9JDZXgK.xRQ0VYG1kGRYz4YW7uELC/qOxs6B2Gy', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('George Moore', 'george.moore@example.com', '$2a$11$XABw.U9VE2xMKS9JDZXgK.xRQ0VYG1kGRYz4YW7uELC/qOxs6B2Gy', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('Helen Taylor', 'helen.taylor@example.com', '$2a$11$XABw.U9VE2xMKS9JDZXgK.xRQ0VYG1kGRYz4YW7uELC/qOxs6B2Gy', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- Verify data insertion
SELECT COUNT(*) as "SellersCount" FROM "Sellers";
SELECT "Name", "Email" FROM "Sellers" ORDER BY "Name";