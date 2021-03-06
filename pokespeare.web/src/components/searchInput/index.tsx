import React, {
    ChangeEvent,
    FormEvent,
    InputHTMLAttributes,
    useState,
  } from 'react';
  
  import { Container } from './styles';
  
  interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
    handleSearch(id: string): any;
  }
  
  const SearchInput: React.FC<InputProps> = ({ handleSearch, ...rest }) => {
    const [inputValue, setInputValue] = useState('');
  
    const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
      event.preventDefault();
      handleSearch(inputValue);
    };
  
    const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
      setInputValue(event.target.value);
    };
  
    return (
      <Container>
        <form onSubmit={handleSubmit}>
          <input data-testid="search" {...rest} onChange={handleChange} value={inputValue} />
          <button data-testid="submit" type="submit">Search</button>
        </form>
      </Container>
    );
  };
  
  export default SearchInput;  