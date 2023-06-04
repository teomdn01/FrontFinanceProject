import { auto } from '@popperjs/core';
import React from 'react';
import Select from 'react-select';

const CustomSelect = ({ onSelectChange }) => {
  const optionsData = [
    { value: 'Alpaca', label: 'Alpaca', logo: '/resources/AlpacaLogo.png' },
    { value: 'Coinbase', label: 'Coinbase', logo: '/resources/Coinbaselogo.png' },
    { value: 'Tradier', label: 'Tradier', logo: '/resources/Tradierlogo.png' },
    { value: 'Freedom', label: 'Freedom', logo: '/resources/Freedomlogo.png' },
    { value: 'Trading212', label: 'Trading212', logo: '/resources/Trading212logo.png' }
  ];

  const CustomOption = ({ innerProps, label, data }) => (
    <div {...innerProps}>
      <img src={data.logo} alt="Logo" width="40" height="40" margin='1px' /> {label}
    </div>
  );

  const customStyles = {
    control: (provided) => ({
      ...provided,
      border: '1px solid #ddd',
      borderRadius: '4px',
      width: '50%',
      margin: 'auto'
    }),
    option: (provided) => ({
      ...provided,
      alignItems: 'center',
      margin: '2px'
    })
  };

  const handleChange = (selectedOption) => {
    onSelectChange(selectedOption.value);
  };

  return (
    <Select
      options={optionsData}
      components={{ Option: CustomOption }}
      styles={customStyles}
      onChange={handleChange}
    />
  );
};

export default CustomSelect;
