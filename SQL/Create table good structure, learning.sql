CREATE TABLE allowed_partner(
	allowed_partner_id BIGINT PRIMARY KEY IDENTITY(1,1),
	allowed_partner_name VARCHAR(50) NOT NULL
)

CREATE INDEX idx_partner_id ON allowed_partner(allowed_partner_id)

CREATE TABLE country(
	country_id BIGINT PRIMARY KEY IDENTITY(1,1),
	country_name VARCHAR(50) NOT NULL,
)

CREATE INDEX idx_country_id ON country(country_id)

CREATE TABLE state(
	state_id BIGINT PRIMARY KEY IDENTITY(1,1),
	country_id BIGINT NOT NULL,
	state_name VARCHAR(50) NOT NULL,

	FOREIGN KEY (country_id) REFERENCES country(country_id)
)

CREATE INDEX idx_country_id ON state(country_id)

CREATE TABLE city(
	city_id BIGINT PRIMARY KEY IDENTITY(1,1),
	state_id BIGINT NOT NULL,
	city_name VARCHAR(50) NOT NULL,

	FOREIGN KEY (state_id) REFERENCES state(state_id)
)

CREATE INDEX idx_state_id ON city(state_id)

CREATE TABLE [address] (
	address_id BIGINT PRIMARY KEY IDENTITY(1,1),
	city_id BIGINT NOT NULL,
	address_field_1 VARCHAR(100),
	address_field_2 VARCHAR(100),
	address_field_3 VARCHAR(100),
	postal_code VARCHAR(20) NOT NULL,

	FOREIGN KEY (city_id) REFERENCES city(city_id)
)

CREATE TABLE partners (
	partner_id BIGINT PRIMARY KEY IDENTITY(1,1),
	allowed_partner_id BIGINT NOT NULL,
	address_id BIGINT NOT NULL,
	partner_name VARCHAR(50) NOT NULL,
	partner_password VARCHAR(50) NOT NULL,

	FOREIGN KEY (allowed_partner_id) REFERENCES allowed_partner(allowed_partner_id),
	FOREIGN KEY (address_id) REFERENCES address(address_id) 
)

CREATE INDEX idx_allowed_partner_id ON partners(allowed_partner_id)
CREATE INDEX idx_address_id ON partners(address_id)

CREATE TABLE unit(
	unit_id BIGINT PRIMARY KEY IDENTITY(1,1),
	unit_name VARCHAR(10) NOT NULL
)

CREATE TABLE item(
	item_id BIGINT PRIMARY KEY IDENTITY(1,1),
	item_code VARCHAR(20) NOT NULL,
	item_name VARCHAR(100) NOT NULL,
	status TINYINT NOT NULL DEFAULT(1),
)

CREATE INDEX idx_item_id ON item(item_id) INCLUDE (item_code, item_name)

CREATE TABLE inventory(
	inventory_id BIGINT PRIMARY KEY IDENTITY(1,1),
	city_id BIGINT NOT NULL,
	inventory_name VARCHAR(50) NOT NULL,
	
	FOREIGN KEY (city_id) REFERENCES city(city_id)
)

CREATE INDEX idx_inventory_id ON inventory(inventory_id, city_id)

CREATE TABLE items_inventory(
	inventory_id BIGINT NOT NULL,
	item_id BIGINT NOT NULL,
	unit_id BIGINT NOT NULL,
	quantity DECIMAL(10,6) NOT NULL DEFAULT(0),
	price DECIMAL(8,2) NOT NULL,

	PRIMARY KEY (inventory_id, item_id, unit_id),
	FOREIGN KEY (inventory_id) REFERENCES inventory(inventory_id),
	FOREIGN KEY (item_id) REFERENCES item(item_id),
	FOREIGN KEY (unit_id) REFERENCES unit(unit_id),
)
