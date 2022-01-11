import React from 'react';
import InfiniteScroll from 'react-infinite-scroll-component';

import { IPokemonListResponse, IPokemonItem } from '../../models/index';
import { Container, Item } from './styles';

interface PokemonListProps {
    selectedPokemon: IPokemonItem;
    pokemonList: IPokemonListResponse;
    loadMorePokemon(): void;
    selectPokemon(id: number): void;
}

const pokemonList: React.FC<PokemonListProps> = ({ selectedPokemon, pokemonList, loadMorePokemon, selectPokemon }) => {
    return (<Container backgroundColor={selectedPokemon.type}>
        <div id="scrollableDiv" role="scrollableDiv">
            <InfiniteScroll
                dataLength={pokemonList.data.length}
                next={loadMorePokemon}
                hasMore={pokemonList.data.length < pokemonList.totalCount}
                loader={<h4>Loading...</h4>}
                endMessage={<p>That's all folks!</p>}
                scrollableTarget="scrollableDiv">
                {
                    pokemonList.data.map(p => (
                        <Item
                            role='item'
                            key={p.id}
                            onClick={() => selectPokemon(p.id)}
                            selected={selectedPokemon.id === p.id}
                        >
                            <p>{`${p.id} - ${p.name}`}</p>
                        </Item>
                    ))
                }
            </InfiniteScroll>
        </div>
    </Container>);
};

export default pokemonList;  