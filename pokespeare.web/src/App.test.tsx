import { fireEvent, render, screen } from '@testing-library/react';

import PokemonName from './components/pokemonName';
import PokemonDescription from './components/pokemonDescription';
import PokemonSprite from './components/pokemonSprite';
import PokemonList from './components/pokemonList';
import SearchInput from './components/searchInput';

import { IPokemonItem, IPokemonListResponse } from './models/index';

describe("Testing Components", () => {

  const pokemon: IPokemonItem = {
    id: 1,
    name: 'Bulbasaur',
    description: 'Bulbasaur can be seen napping in bright sunlight. There is a seed on its back. By soaking up the suns rays, the seed grows progressively larger.',
    sprite: 'https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png',
    translated: true,
    type: "normal"
  };

  const pokemonList: IPokemonListResponse = {
    totalCount: 4,
    data: [
      {
        id: 1,
        name: 'Bulbasaur',
      },
      {
        id: 2,
        name: 'Ivysaur',
      }
    ]
  };

  test('Pokemon name', () => {
    render(<PokemonName selectedPokemon={pokemon} />);
    const h1 = screen.getByText(pokemon.name);
    expect(h1).toBeInTheDocument();
  });

  test('Pokemon description', () => {
    render(<PokemonDescription selectedPokemon={pokemon} />);
    const p = screen.getByRole('description');
    expect(p).toBeInTheDocument();
    expect(p).toHaveTextContent(pokemon.description);
  });

  test('Pokemon translation status value', () => {
    render(<PokemonDescription selectedPokemon={pokemon} />);
    const p = screen.getByRole('translated');
    expect(p).toBeInTheDocument();
    expect(p).toHaveTextContent(`Shakespearean: ${pokemon.translated}`);
  });

  test('Pokemon sprite', () => {
    render(<PokemonSprite selectedPokemon={pokemon} />);
    const img = screen.getByRole('sprite');
    expect(img).toBeInTheDocument();
    expect(img).toHaveStyle({
      background: `url(${pokemon.sprite}) no-repeat center`
    });
  });

  test("Pokemon list has correct count", () => {
    const selectPokemon = jest.fn();
    const loadMorePokemon = jest.fn();

    render(<PokemonList selectedPokemon={pokemon} pokemonList={pokemonList} selectPokemon={selectPokemon} loadMorePokemon={loadMorePokemon} />);

    var items = screen.getAllByRole('item');
    expect(items).toHaveLength(pokemonList.data.length);
    expect(items[0]).toHaveTextContent(`${pokemonList.data[0].id} - ${pokemonList.data[0].name}`);
    expect(items[1]).toHaveTextContent(`${pokemonList.data[1].id} - ${pokemonList.data[1].name}`);
  });

  test("Pokemon list selected item", () => {
    const selectPokemon = jest.fn();
    const loadMorePokemon = jest.fn();

    render(<PokemonList selectedPokemon={pokemon} pokemonList={pokemonList} selectPokemon={selectPokemon} loadMorePokemon={loadMorePokemon} />);

    var items = screen.getAllByRole('item');
    expect(items[0]).toHaveStyle({
      "border-color": "#404040"
    });
    expect(items[1]).not.toHaveStyle({
      "border-color": "#404040"
    });
  });

  test("Pokemon list calls selectPokemon on click", () => {
    const selectPokemon = jest.fn();
    const loadMorePokemon = jest.fn();

    render(<PokemonList selectedPokemon={pokemon} pokemonList={pokemonList} selectPokemon={selectPokemon} loadMorePokemon={loadMorePokemon} />);

    var items = screen.getAllByRole('item');

    fireEvent.click(items[1]);

    expect(selectPokemon).toHaveBeenCalled();
    expect(loadMorePokemon).not.toHaveBeenCalled();
  });

  test("Pokemon list calls loadMorePokemon on scroll", () => {
    const selectPokemon = jest.fn();
    const loadMorePokemon = jest.fn();

    render(<PokemonList selectedPokemon={pokemon} pokemonList={pokemonList} selectPokemon={selectPokemon} loadMorePokemon={loadMorePokemon} />);

    var scrollableDiv = screen.getByRole('scrollableDiv');

    fireEvent.scroll(scrollableDiv);

    expect(selectPokemon).not.toHaveBeenCalled();
    expect(loadMorePokemon).toHaveBeenCalled();
  });

  test("Search Input calls handleSearch on submit", () =>{
    const handleSearch = jest.fn();
    render(<SearchInput handleSearch={handleSearch} />);

    const input = screen.getByRole('search');
    fireEvent.change(input, { target: { value: 'bulbasaur' } });
    expect(input).toHaveValue('bulbasaur');

    const submit = screen.getByRole('submit');
    fireEvent.click(submit);

    expect(handleSearch).lastCalledWith('bulbasaur');
  });
});