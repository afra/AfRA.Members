{
  inputs = {
    nixpkgs.url = "nixpkgs/nixos-24.11";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, flake-utils, nixpkgs, ... }: flake-utils.lib.eachDefaultSystem (system: let
    pkgs = import nixpkgs { inherit system; };
    lib = pkgs.lib;
  in {
    apps.fetch-deps = {
      type = "app";
      program = "${self.packages.${system}.default.fetch-deps}";
    };
    packages.default = pkgs.buildDotnetModule rec {
      name = "AfRA.Members";
      src = ./.;
      projectFile = "AFRA.Members.sln";
      dotnet-sdk = pkgs.dotnetCorePackages.dotnet_8.sdk;
      dotnet-runtime = pkgs.dotnetCorePackages.dotnet_8.sdk;

      executables = [ "AFRA.Members" ];

      nugetDeps = ./deps.json;
      meta = {
        mainProgram = "AFRA.Members";
      };
    };
    devShells.default = pkgs.mkShell {
      packages = [
        pkgs.pkgs.dotnetCorePackages.dotnet_8.sdk
      ];
    };
  });
}
