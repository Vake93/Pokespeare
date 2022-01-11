import styled from "styled-components";
import { Card } from '../../styles/global';

export const Container = styled(Card)`
  width: 100%;

  > div {
    border-radius: 8px;
    padding: 24px;
    border: solid 4px #404040;
    background-color: #fff;
    width: 100%;
    height: 100%;

    display: flex;
    flex-direction: column;
    justify-content: space-between;

    p {
      font-size: 32px;
    }

    p + p {
      margin-top: 16px;
    }
  }
`;