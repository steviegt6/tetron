import { existsSync, readFileSync, writeFileSync } from "fs";
import { join } from "path";

export class Config {
    public url = "https://tetr.io/";
    public windowSize: [number, number] = [1600, 800];

    static read(name?: string | undefined, dir?: string | undefined): Config {
        name ??= "config.json";
        dir ??= process.cwd();

        const fullPath = join(dir, name);

        return existsSync(fullPath) ? JSON.parse(readFileSync(fullPath, "utf8")) : new Config();
    }

    static write(config: Config, name?: string | undefined, dir?: string | undefined): void {
        name ??= "config.json";
        dir ??= process.cwd();

        const fullPath = join(dir, name);
        writeFileSync(fullPath, JSON.stringify(config, null, 4));
    }
}
