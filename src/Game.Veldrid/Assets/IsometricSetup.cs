﻿using System;
using UAlbion.Config;
using UAlbion.Core;
using UAlbion.Core.Veldrid;
using UAlbion.Core.Veldrid.Etm;
using UAlbion.Core.Veldrid.Sprites;
using UAlbion.Core.Veldrid.Textures;
using UAlbion.Core.Visual;
using UAlbion.Game.Assets;
using UAlbion.Game.Scenes;
using UAlbion.Game.State;
using Veldrid;
using Rectangle = UAlbion.Core.Rectangle;

namespace UAlbion.Game.Veldrid.Assets
{
    public static class IsometricSetup
    {
        public static (Container, IsometricBuilder) SetupEngine(
            EventExchange exchange,
            int tileWidth, int tileHeight,
            int baseHeight, int tilesPerRow,
            GraphicsBackend backend, bool useRenderDoc, Rectangle? windowRect)
        {
            if (exchange == null) throw new ArgumentNullException(nameof(exchange));
            var framebuffer = new SimpleFramebuffer((uint)(tileWidth * tilesPerRow), (uint)tileHeight, "FB_Offscreen");
            var builder = new IsometricBuilder(framebuffer, tileWidth, tileHeight, baseHeight, tilesPerRow);

#pragma warning disable CA2000 // Dispose objects before losing scopes
            var config = exchange.Resolve<IGeneralConfig>();
            var shaderCache = new ShaderCache(config.ResolvePath("$(CACHE)/ShaderCache"));
            var sceneRenderer = new SceneRenderer("MainPipeline", framebuffer)
                .AddRenderer(new SpriteRenderer(framebuffer), typeof(VeldridSpriteBatch))
                .AddRenderer(new EtmRenderer(framebuffer), typeof(EtmWindow))
                ;

            foreach (var shaderPath in exchange.Resolve<IModApplier>().ShaderPaths)
                shaderCache.AddShaderPath(shaderPath);

            var engine = new Engine(
                    backend, useRenderDoc, false, windowRect != null, sceneRenderer, windowRect)
                ;

            var renderableSources = new IRenderableSource[]
            {
                new EtmManager(),
                new SpriteManager(),
            };

            var services = new Container("IsometricLayoutServices");
            services
                .Add(shaderCache)
                .Add(framebuffer)
                .Add(sceneRenderer)
                .Add(engine)
                .Add(new SpriteSamplerSource())
                .Add(new TextureSource())
                .Add(new ResourceLayoutSource())
                .Add(new VeldridCoreFactory())
                .Add(new SceneStack())
                .Add(new SceneManager()
                    .AddScene(new EmptyScene())
                    .AddScene((IScene)new IsometricBakeScene()
                        .Add(new PaletteManager())
                        .Add(builder)))
                ;

            foreach (var source in renderableSources)
            {
                if (source is IComponent component)
                    services.Add(component);
                sceneRenderer.AddSource(source);
            }

            return (services, builder);
        }
    }
}