import styled, { css } from 'styled-components';

interface PokemonSpriteProps {
    src: string;
}

export const Container = styled.div<PokemonSpriteProps>`
    ${props =>
        props.src &&
        css`
        background: url(${props.src}) no-repeat center;
      `}
    background-size: contain;
    width: 100%;
    height: 350px;
    image-rendering: pixelated;
  
    transition: background-image 0.3s ease;
  `;