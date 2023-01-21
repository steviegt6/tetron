export interface IConfig {
    get<T>(key: string): T;

    set<T>(key: string, value: T): void;
}
