import React from 'react';
import { setConfiguration } from 'react-grid-system';

import GlobalStyle from './styles/global';
import Home from './pagers/home';

setConfiguration({
  gutterWidth: 10,
  defaultScreenClass: 'lg',
  maxScreenClass: 'lg',
});

const App: React.FC = () => {
  return (
    <>
      <Home />
      <GlobalStyle />
    </>
  );
};

export default App;
