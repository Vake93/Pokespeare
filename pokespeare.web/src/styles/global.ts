import styled, { createGlobalStyle, css } from 'styled-components';
import colors, { IPokemonTypeName } from '../util/colours';
import { shade } from 'polished';

export default createGlobalStyle`
  * {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
    outline: 0;
  }

  body {
    background-color: #E0E8E8;
    color: #404040;
    -webkit-font-smoothing: antialiased;
  }


  body, input, button {
    font-family: 'VT323', monospace;
    font-size: 16px;
  }

  h1, h2, h3, h4, h5, strong {
    font-weight: 500;
  }

  button {
    cursor: pointer;
  }
`;

export interface CardProps {
  backgroundColor?: IPokemonTypeName;
}

export const Card = styled.div<CardProps>`
border-radius: 8px;
padding: 8px 10px 10px 8px;
border: solid 4px #404040;
background-color: #c4c4c4;
width: 100%;
image-rendering: pixelated;

${props =>
  props.backgroundColor &&
  css`
    background-color: ${colors[props.backgroundColor]};
    box-shadow: -2px -2px ${shade(0.4, colors[props.backgroundColor])} inset;
  `}

transition: background-color 0.2s;
`;