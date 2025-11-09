import { PUBLIC_API_URL } from '$env/static/public';
import type { Movie } from '$lib/types/movie.model';
import type { Result } from '$lib/types/result.model';
export interface CreateMovieRequest {
    title: string;
    description: string;
    pricePerDay: number;
    collateralValue: number;
}

export interface UpdateMovieRequest {
    title: string;
    description: string;
    pricePerDay: number;
    collateralValue: number;
}

class MoviesService {
    private baseUrl = `${PUBLIC_API_URL}/movies`;

    async getMovies(): Promise<Result<Movie[]>> {
        try {
            const response = await fetch(this.baseUrl);

            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Failed to load movies.' }));
                return { success: false, error };
            }

            const movies = await response.json();
            return { success: true, data: movies ?? [] };
        } catch (err) {
            return { success: false, error: { message: 'Failed to load movies.' } };
        }
    }

    async getMovieById(movieId: string): Promise<Result<Movie>> {
        try {
            const response = await fetch(`${this.baseUrl}/${movieId}`);

            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Failed to load movie details.' }));
                return { success: false, error };
            }

            const movie = await response.json();

            if (!movie) {
                return { success: false, error: { message: 'Failed to deserialize movie.' } };
            }

            return { success: true, data: movie };
        } catch (err) {
            return { success: false, error: { message: 'Failed to load movie details.' } };
        }
    }

    async createMovie(request: CreateMovieRequest): Promise<Result<void>> {
        try {
            const response = await fetch(this.baseUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(request),
            });

            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Failed to create movie.' }));
                return { success: false, error };
            }

            return { success: true, data: undefined };
        } catch (err) {
            return { success: false, error: { message: 'Failed to create movie.' } };
        }
    }

    async updateMovie(movieId: string, request: UpdateMovieRequest): Promise<Result<void>> {
        try {
            const response = await fetch(`${this.baseUrl}/${movieId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(request),
            });

            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Failed to update movie.' }));
                return { success: false, error };
            }

            return { success: true, data: undefined };
        } catch (err) {
            return { success: false, error: { message: 'Failed to update movie.' } };
        }
    }

    async deleteMovie(movieId: string): Promise<Result<void>> {
        try {
            const response = await fetch(`${this.baseUrl}/${movieId}`, {
                method: 'DELETE',
            });

            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Failed to delete movie.' }));
                return { success: false, error };
            }

            return { success: true, data: undefined };
        } catch (err) {
            return { success: false, error: { message: 'Failed to delete movie.' } };
        }
    }

    async returnMovie(movieId: string, clientId: string): Promise<Result<void>> {
        try {
            const response = await fetch(`${this.baseUrl}/${movieId}/return?clientId=${clientId}`, {
                method: 'POST',
            });

            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Failed to return movie.' }));
                return { success: false, error };
            }

            return { success: true, data: undefined };
        } catch (err) {
            return { success: false, error: { message: 'Failed to return movie.' } };
        }
    }
}

export const moviesService = new MoviesService();