import { shade } from "polished";
import styled, { css } from "styled-components";
import { Card } from '../../styles/global';

export const Container = styled(Card)`
  height: 100%;
  max-height: 635px;
  margin-top: 8px;
  margin-bottom: 8px;

  > div {
    border-radius: 8px;
    overflow-y: auto;
    padding: 8px;
    border: solid 4px #404040;
    background-color: #fff;
    width: 100%;
    height: 100%;
    div > p {
      text-align: center;
      color: #bdbdbd;
      margin-top: 32px;
    }
  }
`;

interface ItemProps {
  selected: boolean;
}

export const Item = styled.button<ItemProps>`
  border: 4px solid transparent;
  border-radius: 4px;
  background: transparent;
  width: 100%;
  font-size: 24px;
  padding: 8px;
  margin-bottom: 8px;
  text-align: left;
  text-transform: capitalize;

  color: #404040;

  &:hover {
    color: ${shade(0.8, '#404040')};
    border-color: #404040;
  }

  ${props =>
    props.selected &&
    css`
      border-color: #404040;
    `}
`;