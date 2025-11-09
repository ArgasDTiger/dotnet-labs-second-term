import { PUBLIC_API_URL } from '$env/static/public';
import type { Client } from '$lib/types/client.model';
import type { Result } from '$lib/types/result.model';

export interface CreateClientRequest {
    firstName: string;
    middleName: string;
    lastName: string;
    phoneNumber: string;
    homeAddress: string;
    passportSeries: string | null;
    passportNumber: string;
}

export interface UpdateClientRequest {
    firstName: string;
    middleName: string;
    lastName: string;
    phoneNumber: string;
    homeAddress: string;
    passportSeries: string | null;
    passportNumber: string;
}

class ClientsService {
    private baseUrl = `${PUBLIC_API_URL}/clients`;

    async getClients(): Promise<Result<Client[]>> {
        try {
            const response = await fetch(this.baseUrl);

            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Failed to load clients.' }));
                return { success: false, error };
            }

            const clients = await response.json();
            return { success: true, data: clients ?? [] };
        } catch (err) {
            return { success: false, error: { message: 'Failed to load clients.' } };
        }
    }

    async getClientById(clientId: string): Promise<Result<Client>> {
        try {
            const response = await fetch(`${this.baseUrl}/${clientId}`);

            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Failed to load client details.' }));
                return { success: false, error };
            }

            const client = await response.json();

            if (!client) {
                return { success: false, error: { message: 'Failed to deserialize client.' } };
            }

            return { success: true, data: client };
        } catch (err) {
            return { success: false, error: { message: 'Failed to load client details.' } };
        }
    }

    async createClient(request: CreateClientRequest): Promise<Result<void>> {
        try {
            const response = await fetch(this.baseUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(request),
            });

            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Failed to create client.' }));
                return { success: false, error };
            }

            return { success: true, data: undefined };
        } catch (err) {
            return { success: false, error: { message: 'Failed to create client.' } };
        }
    }

    async updateClient(clientId: string, request: UpdateClientRequest): Promise<Result<void>> {
        try {
            const response = await fetch(`${this.baseUrl}/${clientId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(request),
            });

            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Failed to update client.' }));
                return { success: false, error };
            }

            return { success: true, data: undefined };
        } catch (err) {
            return { success: false, error: { message: 'Failed to update client.' } };
        }
    }

    async deleteClient(clientId: string): Promise<Result<void>> {
        try {
            const response = await fetch(`${this.baseUrl}/${clientId}`, {
                method: 'DELETE',
            });

            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Failed to delete client.' }));
                return { success: false, error };
            }

            return { success: true, data: undefined };
        } catch (err) {
            return { success: false, error: { message: 'Failed to delete client.' } };
        }
    }
}

export const clientsService = new ClientsService();