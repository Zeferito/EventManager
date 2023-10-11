/*
 * MIT License
 *
 * Copyright (c) 2023 Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
package dev.dust.net.eventmanager.server.services.impl;

import dev.dust.net.eventmanager.server.entities.Cliente;
import dev.dust.net.eventmanager.server.repositories.ClienteRepository;
import dev.dust.net.eventmanager.server.services.interfaces.ClienteService;
import java.util.List;
import java.util.Optional;
import org.springframework.stereotype.Service;

@Service
public class ClienteServiceImpl implements ClienteService {

    private ClienteRepository clienteRepository;

    public ClienteServiceImpl(ClienteRepository clienteRepository) {
        this.clienteRepository = clienteRepository;
    }

    @Override
    public List<Cliente> getAll() {
        return clienteRepository.findAll();

    }

    @Override
    public Cliente getById(Integer id) {
        Optional<Cliente> cliente = clienteRepository.findById(id);

        if (!cliente.isPresent()) {
            System.out.println("No hay clientes registrados por ese id");
            return null;
        }
        return cliente.get();
    }

    @Override
    public Cliente insert(Cliente cliente) {
        return clienteRepository.save(cliente);
    }

    @Override
    public Cliente update(Integer id, Cliente cliente) {
        //Buscamos al cliente
        Optional<Cliente> clienteExistenteOpcional = clienteRepository.findById(id);

        if (!clienteExistenteOpcional.isPresent()) {
            System.out.println("No hay clientes registrados por ese id");
            return null;
        }

        Cliente clienteExistente = clienteExistenteOpcional.get();
        clienteExistente.setNombre(cliente.getNombre());
        clienteExistente.setTelefono(cliente.getTelefono());
        clienteExistente.setEvento(cliente.getEvento());

        return clienteRepository.save(clienteExistente);
    }

    @Override
    public Boolean delete(Integer id) {
        clienteRepository.deleteById(id);

        return clienteRepository.existsById(id);
    }

}
