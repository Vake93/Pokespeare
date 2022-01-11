import { IPokemonTypeName } from '../util/colours';

export interface IPokemonListResponse {
    totalCount: number;
    data: IPokemonListItem[];
}

export interface IPokemonResponse {
    data: IPokemonItem;
}

export interface IPokemonListItem {
    id: number;
    name: string;
}

export interface IPokemonItem {
    id: number;
    name: string;
    description: string;
    sprite: string;
    type: IPokemonTypeName;
    translated: boolean;
}